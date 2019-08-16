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
using System.IO.Ports;
using System.Threading;
using Websocket;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Websocket.Client;
using System.Net.WebSockets;
using System.Collections;

namespace MoonBot
{
    
    class Program
    {

        public static readonly string _password = ConfigurationManager.AppSettings["password"];
        public static readonly SerialPort _port = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);
        public static readonly string _broadcasterName = ConfigurationManager.AppSettings["broadcaster"];
        public static List<string> _mods = new List<string>();
        public static List<string> _viewers = new List<string>();
        public static IrcClient _irc;
        public static StringBuilder _commandsText;
        public static UserO _broadcaster = new UserO();
        public static ChannelO _channel = new ChannelO();
        public static List<string> _moderators = new List<string>();
        public static string[] _command;
        public static string _username;
        public static string _channelId;
        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            _port.Open();
            _port.Write("m");
            _port.Close();
        }

        static void Main(string[] args)
        {
            #region LoadChannel
                _channel = ChannelD.GetChannel();
                _channelId = _channel._id;
            #endregion

            _irc = new IrcClient("irc.twitch.tv", 6667, ChatBot.botName, _password, _channel.name);
            TwitchApi api = new TwitchApi();
            TmiApi tmi = new TmiApi();
            ViewerList chatters = new ViewerList();
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
            TwitchSocket twitchSocket = new TwitchSocket();
            ChatBot.init();
            
            #region LoadBroadCasterInfo
            _broadcaster = UserD.GetUser(_broadcasterName);
            #endregion

            _moderators.Add("gaelLevel");
            _moderators.Add("terror_seeds");
            _moderators.Add("nebulea");
            _moderators.Add("novaevermoon");

            #region LoadCommands
            List<CommandO> commands = new List<CommandO>();
            commands = CommandD.LoadCommands();
            LaunchTimer launchTimer = new LaunchTimer(_irc);
            foreach (CommandO command in commands)
            {
                if (command.type == "timed")
                {
                    launchTimer.createTimer(command);
                }
            }


            if (commands.Count != 0)
            {
                _commandsText = new StringBuilder();
                foreach (CommandO command in commands)
                {
                    if (command.userLevel == "everyone" && command.timer == 0)
                    {
                        _commandsText.Append("!" + command.keyword + ", ");

                    }
                }
                _commandsText.Length = _commandsText.Length - 2;
            }


            #endregion

            chatters = tmi.getViewerList(_channel);

            //_mods = tmi.getMods(chatters);
            _viewers = tmi.getViewers(chatters);
            twitchSocket.ClientWebSocket = twitchSocket.WebSocketConnectAsync().Result;
            var testSocket = twitchSocket.WhisperSubscribeAsync(twitchSocket.ClientWebSocket, _broadcaster.users[0]._id);
            //ClientWebSocket webSocket = twitchSocket.WebSocketConnectAsync().Result;

            //var testWebSocket = twitchSocket.WhisperSubscribeAsync(twitchSocket.whisperWebSocket,_broadcaster.users[0]._id).Result;
        

            PingSender ping = new PingSender(_irc);
            ping.Start();



            while (true)
            {
                string fullMessage = _irc.ReadMessage();
                CommandO foundCommand = new CommandO();
                if (fullMessage.Contains("PRIVMSG"))
                {
                    _username = UserD.GetUsername(fullMessage);
                    string message = ChatBot.GetMessage(fullMessage);
                    //bool isMod = chatters.chatters.moderators.Contains(username);
                    UserO user = UserD.GetUser(_username);
                    UserD.InsertUser(user);

                    bool isSubscriber = SubscriberD.IsSubscriber(user.users[0]._id, _broadcaster.users[0]._id);

                    if (isSubscriber == false)
                    {
                        bool link = ChatBot.checkLink(message);
                        if (link == true)
                        {
                            _irc.WriteChatMessage(".timeout " + _username + " 15");
                            _irc.WriteChatMessage("Posting links is not allowed here for non-subs, if you think this link might interest me, just whisper me or one of my mods ♡");
                        }
                    }

                    char firstCharacter = message[0];

                    try
                    {
                        if (firstCharacter == '!')
                        {

                            string commandMessage = message.Substring(message.IndexOf('!') + 1);
                            
                            bool kappamonCommand = CommandD.IsKappamonCommand(commandMessage);

                            if (!kappamonCommand)
                            {
                                if (commandMessage.Contains(" "))
                                {
                                    _command = commandMessage.Split(' ');
                                    commandMessage = _command[0];
                                }

                                if (commands.Any(c => c.keyword == commandMessage))
                                {
                                    foundCommand = commands.Single(c => c.keyword == commandMessage);
                                    if (_command != null)
                                    {
                                        foundCommand.parameterList["username"] = _command[1];
                                    }
                                    else if (_command == null && foundCommand.parameters != 0)
                                    {
                                        Type testType = typeof(Program);
                                        Dictionary<string, string> newDic = new Dictionary<string, string>();

                                        foreach(KeyValuePair<string,string> dic in foundCommand.parameterList)
                                        {
                                            var fieldInfo = testType.GetField(dic.Key.ToString(), BindingFlags.Static | BindingFlags.Public).GetValue(testType) as IEnumerable;
                                            newDic.Add(dic.Key.ToString(), fieldInfo.ToString());
                                        }

                                        foundCommand.parameterList = newDic;
                                    }

                                    if (foundCommand.userLevel == "moderator" && !_moderators.Contains(_username))
                                    {
                                        _irc.WriteChatMessage("You are not allowed to use this command !");
                                    }
                                    else
                                    {
                                        DateTime date = DateTime.Now;

                                        if (foundCommand.startedTime.AddMilliseconds(foundCommand.cooldown) < DateTime.Now)

                                        {
                                            foundCommand.startedTime = DateTime.Now;
                                            if (foundCommand.keyword == "commands")
                                            {
                                                foundCommand.message += _commandsText;
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
                                                                    foundCommand.request = string.Format(foundCommand.request, _username);
                                                                    foundCommand.message = string.Format(foundCommand.message, _username);
                                                                    break;
                                                            }
                                                        }

                                                        if (foundCommand.request.Contains("SELECT"))
                                                        {
                                                            Tuple<int, string> test = CommandD.ExecuteSelectCommand(foundCommand.request);
                                                            if (foundCommand.message.Contains("@"))
                                                            {
                                                                _irc.WriteChatMessage(foundCommand.message.Replace("@", test.Item1.ToString()));
                                                            }
                                                            else
                                                            {
                                                                _irc.WriteChatMessage(test.Item2);
                                                            }
                                                        }
                                                        else if (foundCommand.request.Contains("UPDATE"))
                                                        {
                                                            Tuple<int, string> result = CommandD.ExecuteUpdateCommand(foundCommand.request, foundCommand.message);
                                                            mySqlConnection.Close();
                                                            if (result.Item1 < 0)
                                                            {

                                                            }
                                                            else
                                                            {
                                                                _irc.WriteChatMessage(result.Item2);
                                                            }
                                                        }
                                                    }
                                                break;
                                                case "regular":
                                                    if(foundCommand.message.Contains("{"))
                                                    {
                                                        _irc.WriteChatMessage(string.Format(foundCommand.message,_username));
                                                    }
                                                    else
                                                    {
                                                        _irc.WriteChatMessage(foundCommand.message);
                                                    }
                                                break;
                                                case "api":
                                                    MethodInfo mInfo;
                                                    Type type = Assembly.Load("MoonBot_Data").GetType(foundCommand.file, false, true);
                                                    mInfo = type.GetMethod(foundCommand.message);
                                                    object[] parameters;

                                                    if (foundCommand.parameters == 0)
                                                    {
                                                        parameters = new object[] {};
                                                    }
                                                    else
                                                    {
                                                        Type testType = typeof(Program);
                                                        //var fieldInfo = testType.GetField(foundCommand.parameter,BindingFlags.Static | BindingFlags.Public); //.GetValue(testType) as IEnumerable;
                                                        ////var paramValue =  this.GetType().GetField(foundCommand.parameter).GetValue(this) as IEnumerable;

                                                        parameters = new object[] { foundCommand.parameterList };
                                                    }

                                                    object apiAnswer = mInfo.Invoke(null, parameters);
                                                    _irc.WriteChatMessage(apiAnswer.ToString());
                                                break;
                                                case "moonlights":
                                                    _irc.WriteChatMessage("Switching color to : " + foundCommand.keyword);
                                                    _port.Open();
                                                    _port.Write(foundCommand.message);
                                                    _port.Close();
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            TimeSpan span = date - foundCommand.startedTime;
                                            int ms = (int)span.TotalMilliseconds;
                                            if (ms <= foundCommand.cooldown)
                                            {
                                                _irc.WriteChatMessage("This command is in cooldown right now, be patient !");
                                            }
                                            else
                                            {
                                                _irc.WriteChatMessage(foundCommand.message);
                                                foundCommand.startedTime = DateTime.Now;
                                            }
                                        }
                                    }
                                }   
                                else
                                {
                                    _irc.WriteChatMessage("This command does not exist, type !commands to know what commands are available");
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




