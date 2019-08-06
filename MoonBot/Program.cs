﻿using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Configuration;
using Moonbot_Objects;
using Moonbot_Objects.Channel;
using Moonbot_Objects.Command;
using Moonbot_Objects.User;
using MoonBot_Data;
using System.IO.Ports;
using System.Threading;
using Websocket;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Websocket.Client;
using System.Net.WebSockets;

namespace MoonBot
{
    class Program
    {
        static readonly string password = ConfigurationManager.AppSettings["password"];
        static readonly SerialPort port = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);
        static readonly string broadcasterName = ConfigurationManager.AppSettings["broadcaster"];
        static List<string> mods = new List<string>();
        static List<string> viewers = new List<string>();
        static IrcClient irc;
        static StringBuilder commandsText;
        static UserO broadcaster = new UserO();
        static ChannelO channel = new ChannelO();

        static void Main(string[] args)
        {
            TwitchSocket twitchSocket = new TwitchSocket();
            ChatBot.init();

            #region LoadChannel
                channel = ChannelD.getChannel();
            #endregion

            #region LoadBroadCasterInfo
                broadcaster = UserD.getUser(broadcasterName);
            #endregion

            

            irc = new IrcClient("irc.twitch.tv", 6667, ChatBot.botName, password, channel.name);

            
            
            TwitchApi api = new TwitchApi();
            TmiApi tmi = new TmiApi();
            
            ViewerList chatters = new ViewerList();

            chatters = tmi.getViewerList(channel);

            mods = tmi.getMods(chatters);
            viewers = tmi.getViewers(chatters);
            ClientWebSocket webSocket = twitchSocket.WebSocketConnectAsync().Result;
            twitchSocket.BitsSubscribe(webSocket);

            System.Timers.Timer timerPing = new System.Timers.Timer(30000);
            timerPing.Start();
            timerPing.Elapsed += (sender, e) => twitchSocket.SendPingAsync(webSocket).Wait();



            TimeSpan timeSpan = new TimeSpan(0, 0, 0, 30);

            //if(webSocket.State == WebSocketState.Open)
            //{
            //    var result = PeriodicSendPing(timeSpan, CancellationToken.None, webSocket).Result;
            //}



            #region LoadCommands
            List<CommandO> commands = new List<CommandO>();
            commands = CommandD.loadCommands();
            LaunchTimer launchTimer = new LaunchTimer(irc);
            foreach (CommandO command in commands)
            {
                if (command.type == "timed")
                {
                    launchTimer.createTimer(command);
                }
            }


            if(commands.Count !=0)
            {
                 commandsText = new StringBuilder();
                foreach (CommandO command in commands)
                {
                    if (command.userLevel == "everyone" && command.timer == 0)
                    {
                        commandsText.Append("!" + command.keyword + ", ");

                    }
                }
                commandsText.Length = commandsText.Length - 2;
            }
            

            #endregion

            PingSender ping = new PingSender(irc);
            ping.Start();

            while (true)
            {

                chatters = tmi.getViewerList(channel);

                string fullMessage = irc.ReadMessage();
                CommandO foundCommand = new CommandO();
                if (fullMessage.Contains("PRIVMSG"))
                {
                    string username = UserD.GetUsername(fullMessage);
                    string message = ChatBot.GetMessage(fullMessage);
                    bool isMod = chatters.chatters.moderators.Contains(username);
                    UserO user = UserD.getUser(username);
                    UserD.insertUser(user);

                    //Follower apifollower = api.GetUserFollower(user);

                    bool isSubscriber = SubscriberD.isSubscriber(user.users[0]._id,broadcaster.users[0]._id);

                    if (isSubscriber == false)
                    {
                        bool link = ChatBot.checkLink(message);
                        if (link == true)
                        {
                            irc.WriteChatMessage(".timeout " + username + " 15");
                            irc.WriteChatMessage("Posting links is not allowed here for non-subs, if you think this link might interest me, just whisper me or one of my mods ♡");
                        }
                    }
 
                    char firstCharacter = message[0];

                    try
                    {
                        if (firstCharacter == '!')
                        {
                            string commandMessage = message.Substring(message.IndexOf('!') + 1);
                            bool kappamonCommand = CommandD.isKappamonCommand(commandMessage);

                            if (!kappamonCommand)
                            {
                                if (commandMessage.Contains(" "))
                                {
                                    string[] command = commandMessage.Split(' ');
                                }
                                else
                                {
                                    if (commands.Any(c => c.keyword == commandMessage))
                                    {
                                        foundCommand = commands.Single(c => c.keyword == commandMessage);

                                        DateTime date = DateTime.Now;

                                        if (foundCommand.startedTime.AddMilliseconds(foundCommand.cooldown) < DateTime.Now)
                                        {
                                            foundCommand.startedTime = DateTime.Now;
                                            if (foundCommand.keyword == "commands")
                                            {
                                                foundCommand.message += commandsText;
                                            }

                                            switch (foundCommand.type)
                                            {
                                                case "request":
                                                    string query = foundCommand.request;
                                                    using (MySqlConnection mySqlConnection = new MySqlConnection(""/*connectionString*/))
                                                    {
                                                        if (foundCommand.condition != "")
                                                        {
                                                            switch (foundCommand.condition)
                                                            {
                                                                case "userName":
                                                                    foundCommand.request = string.Format(foundCommand.request, username);
                                                                    foundCommand.message = string.Format(foundCommand.message, username);
                                                                    break;
                                                            }
                                                        }

                                                        if (foundCommand.request.Contains("SELECT"))
                                                        {
                                                            

                                                            Tuple<int, string> test = CommandD.executeSelectCommand(foundCommand.request);
                                                            if (foundCommand.message.Contains("@"))
                                                            {
                                                                irc.WriteChatMessage(foundCommand.message.Replace("@", test.Item1.ToString()));
                                                            }
                                                            else
                                                            {
                                                            irc.WriteChatMessage(test.Item2);
                                                            }
                                                        }
                                                        else if (foundCommand.request.Contains("UPDATE"))
                                                        {
                                                            Tuple<int, string> result = CommandD.executeUpdateCommand(foundCommand.request, foundCommand.message);
                                                            mySqlConnection.Close();
                                                            if (result.Item1 < 0)
                                                            {

                                                            }
                                                            else
                                                            {
                                                                irc.WriteChatMessage(result.Item2);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                case "regular":
                                                    irc.WriteChatMessage(foundCommand.message);
                                                    break;
                                                case "api":
                                                    MethodInfo mInfo;
                                                    Type type = Assembly.Load("MoonBot_Data").GetType(foundCommand.file, false, true);
                                                    mInfo = type.GetMethod(foundCommand.message);
                                                    object[] parameters;

                                                    if (foundCommand.parameter == 0)
                                                    {
                                                        parameters = new object[] {/* apifollower, username*/ };
                                                    }
                                                    else
                                                    {
                                                        parameters = new object[] { /*apifollower, username*/ };
                                                    }

                                                    object apiAnswer = mInfo.Invoke(null, parameters);
                                                    irc.WriteChatMessage(apiAnswer.ToString());
                                                    break;
                                                case "moonlights":
                                                    irc.WriteChatMessage("Switching color to : " + foundCommand.keyword);
                                                    port.Open();
                                                    port.Write(foundCommand.message);
                                                    port.Close();

                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            TimeSpan span = date - foundCommand.startedTime;
                                            int ms = (int)span.TotalMilliseconds;
                                            if (ms <= foundCommand.cooldown)
                                            {
                                                irc.WriteChatMessage("This command is in cooldown right now, be patient !");
                                            }
                                            else
                                            {
                                                irc.WriteChatMessage(foundCommand.message);
                                                foundCommand.startedTime = DateTime.Now;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        irc.WriteChatMessage("This command does not exist, type !commands to know what commands are available");
                                    }
                                }
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        StringBuilder sb = new StringBuilder(DateTime.Now.ToString("dd-MM-yyyy") + " : " + ex.Message);
                        Console.WriteLine(sb);
                    }
                }
            }
        }

        
    }
}




