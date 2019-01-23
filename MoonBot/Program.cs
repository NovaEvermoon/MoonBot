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

namespace MoonBot
{
    class Program
    {
        public static readonly string[] kappamonCommands = { "song", "feed", "meow" };

        static void Main(string[] args)
        {
            IrcClient irc = new IrcClient("irc.twitch.tv", 6667, ChatBot.botName, ChatBot.password, ChatBot.broadcasterName);
            TwitchApi api = new TwitchApi();
            TmiApi tmi = new TmiApi();

            Example chatters = new Example();


            String connString = "Server=127.0.0.1;Database=MoonBot;port=3306;User Id=root;password=";

            MySqlConnection mySqlConnection = new MySqlConnection(connString);
            mySqlConnection.Open();
            string request = "SELECT * FROM command WHERE command_status != 0";
            MySqlCommand commandGetCommands = new MySqlCommand(request, mySqlConnection);
            MySqlDataReader reader = commandGetCommands.ExecuteReader();

            List<Command> commands = new List<Command>();
            List<Command> timedCommands = new List<Command>();


            //string test = api.GetTeamMember("novaevermoon", "theicewalkers");
            //Channel channel = api.getChannel();

            while (reader.Read())
            {
                Command chatCommand = new Command();

                chatCommand.id = reader.GetInt32(0);
                chatCommand.keyword = reader.GetString(1);
                chatCommand.message = reader.GetString(2);
                chatCommand.userLevel = reader.GetString(3);
                chatCommand.cooldown = reader.GetInt32(4);
                chatCommand.status = reader.GetBoolean(5);
                chatCommand.timer = reader.GetInt32(6);
                chatCommand.description = reader.GetString(7);
                chatCommand.type = reader.GetString(8);
                chatCommand.request = reader.GetString(9);

                switch (chatCommand.type)
                {
                    case "regular":
                        commands.Add(chatCommand);
                        break;
                    case "timed":
                        timedCommands.Add(chatCommand);
                        break;
                    case "request":
                        commands.Add(chatCommand);
                        break;
                    case "api":
                        commands.Add(chatCommand);
                        break;
                }

            }

            LaunchTimer launchTimer = new LaunchTimer(irc);
            foreach(Command timedCommand in timedCommands)
            {
                launchTimer.createTimer(timedCommand);
            }


            mySqlConnection.Close();

            string commandsText = "";

            foreach (Command command in commands)
            {
                if (command.userLevel == "everyone" && command.timer == 0)
                {
                    commandsText += "!" + command.keyword + ", ";

                }
            }

            

            PingSender ping = new PingSender(irc);
            ping.Start();

            while (true)
            {

                chatters = tmi.getMods();

                string fullMessage = irc.ReadMessage();
                if (fullMessage.Contains("PRIVMSG"))
                {
                    string username = ChatBot.GetUsername(fullMessage);
                    string message = ChatBot.GetMessage(fullMessage);
                    bool isMod = chatters.chatters.moderators.Contains(username);
                    User user = api.getUser(username);
                    Subscription sub = api.getUserSubscriber(user);

                    Follower apifollower = api.GetUserFollower(user);


                    if (sub.user == null)
                    {
                        bool link = ChatBot.checkLink(message);
                        if (link == true)
                        {
                            irc.WriteChatMessage(".timeout " + username + " 15");
                            irc.WriteChatMessage("Posting links is not allowed here for non-subs, if you think this link might interest me, just whisper me or one of my mods ♡");
                        }

                    }

                    char firstCharacter = message[0];
                    if (firstCharacter == '!')
                    {
                        string commandMessage = message.Substring(message.IndexOf('!') + 1);
                        if (commandMessage.Contains(" "))
                        {
                            string fullcommand = commandMessage.Substring(message.IndexOf(' '));
                        }
                        else
                        {
                            if (commands.Any(c => c.keyword == commandMessage) || kappamonCommands.Contains(commandMessage))
                            {
                                Command foundCommand = commands.Single(c => c.keyword == commandMessage);


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
                                            mySqlConnection.Open();
                                            MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlConnection);

                                            if (foundCommand.request.Contains("SELECT"))
                                            {
                                                var result = mySqlCommand.ExecuteScalar();
                                                mySqlConnection.Close();
                                                if (result == null)
                                                {
                                                    irc.WriteChatMessage("There was a problem executing that command");
                                                }
                                                else
                                                {
                                                    if (foundCommand.message.Contains("@"))
                                                    {
                                                        irc.WriteChatMessage(foundCommand.message.Replace("@", result.ToString()));
                                                    }
                                                }
                                            }
                                            else if (foundCommand.request.Contains("UPDATE"))
                                            {
                                                int result = mySqlCommand.ExecuteNonQuery();
                                                mySqlConnection.Close();
                                                if (result < 0)
                                                {

                                                }
                                                else
                                                {
                                                    irc.WriteChatMessage(foundCommand.message);
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
                                                parameters = new object[] { apifollower, username };
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




