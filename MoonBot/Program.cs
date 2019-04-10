using MySql.Data.MySqlClient;
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
using MoonBot_Data;
using MoonBot_Data.Channel;
using Moonbot_Objects.Channel;
using Moonbot_Objects.Command;
using Moonbot_Objects.User;

namespace MoonBot
{
    class Program
    {
        static readonly string password = ConfigurationManager.AppSettings["password"];

        static void Main(string[] args)
        {
            #region LoadChannel
                ChannelO channel = ChannelD.getChannel();
            #endregion


            IrcClient irc = new IrcClient("irc.twitch.tv", 6667, ChatBot.botName, password, channel.name);
            TwitchApi api = new TwitchApi();
            TmiApi tmi = new TmiApi();
            
            Examplet chatters = new Examplet();

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

            string commandsText = "";

            foreach (CommandO command in commands)
            {
                if (command.userLevel == "everyone" && command.timer == 0)
                {
                    commandsText += "!" + command.keyword + ", ";

                }
            }
            #endregion
            //"KitanaJAne"
            FollowerO followers = ChannelD.getChannelFollowers(24, channel);

            PingSender ping = new PingSender(irc);
            ping.Start();

            while (true)
            {

                chatters = tmi.getMods(channel);

                string fullMessage = irc.ReadMessage();
                if (fullMessage.Contains("PRIVMSG"))
                {
                    string username = UserD.GetUsername(fullMessage);
                    string message = ChatBot.GetMessage(fullMessage);
                    bool isMod = chatters.chatters.moderators.Contains(username);
                    UserO user = UserD.getUser(username);

                    UserD.insertUser(user);
                    
                    //Follower apifollower = api.GetUserFollower(user);


                    //if (sub.user == null)
                    //{
                    //    bool link = ChatBot.checkLink(message);
                    //    if (link == true)
                    //    {
                    //        irc.WriteChatMessage(".timeout " + username + " 15");
                    //        irc.WriteChatMessage("Posting links is not allowed here for non-subs, if you think this link might interest me, just whisper me or one of my mods ♡");
                    //    }

                    //}

                    char firstCharacter = message[0];
                    if (firstCharacter == '!')
                    {   
                        string commandMessage = message.Substring(message.IndexOf('!') + 1);
                        bool kappamonCommand = CommandD.isKappamonCommand(commandMessage);

                        if (!kappamonCommand)
                        {
                            if (commandMessage.Contains(" "))
                            {
                                string fullcommand = commandMessage.Substring(message.IndexOf(' '));
                            }
                            else
                            {
                                if (commands.Any(c => c.keyword == commandMessage))
                                {
                                    CommandO foundCommand = commands.Single(c => c.keyword == commandMessage);

                                    DateTime testDate = new DateTime(1, 1, 1);
                                    DateTime date = DateTime.Now;

                                    if (foundCommand.startedTime == testDate)
                                    {

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
                                                    if (foundCommand.request.Contains("SELECT"))
                                                    {
                                                        Tuple<int, string> test = CommandD.executeSelectCommand(foundCommand.request);
                                                        if (test.Item1 == 0)
                                                        {
                                                            irc.WriteChatMessage(test.Item2);
                                                        }
                                                        else
                                                        {
                                                            if (foundCommand.message.Contains("@"))
                                                            {
                                                                irc.WriteChatMessage(foundCommand.message.Replace("@", test.Item1.ToString()));
                                                            }
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
                                                Type type = Type.GetType("MoonBot.TwitchApi", false);
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

                                                object apiAnswer = mInfo.Invoke(api, parameters);
                                                irc.WriteChatMessage(apiAnswer.ToString());
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
            }
        }
    }
}




