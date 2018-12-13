using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

            // Connection String
            String connString = "Server=127.0.0.1;Database=MoonBot;port=3306;User Id=root;password=";

            MySqlConnection mySqlConnection = new MySqlConnection(connString);
            mySqlConnection.Open();
            string request = "SELECT * FROM command";
            MySqlCommand commandGetCommands = new MySqlCommand(request, mySqlConnection);
            MySqlDataReader reader = commandGetCommands.ExecuteReader();

            List<Command> commands = new List<Command>();
            List<Command> timedCommands = new List<Command>();


            
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

                switch(chatCommand.type)
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
                }

            }

            mySqlConnection.Close();

            string commandsText = "";

            foreach(Command command in commands)
            {
                if(command.userLevel == "everyone" && command.timer ==0)
                {
                    commandsText += "!" + command.keyword + ", ";
                    
                }
            }

            JsonFollowersAnswer JsonAnswer = new JsonFollowersAnswer();
            int count = 0;

            JsonAnswer = irc.GetFollowersAnswer(ChatBot.channelId, ChatBot.clientID, count, "");

            count = JsonAnswer.data.Count();

            JsonAnswer = irc.GetFollowersAnswer(ChatBot.channelId, ChatBot.clientID, count, JsonAnswer.pagination.cursor);


            Timer timer = new Timer(timedCommands[0].timer);
            Timer timer2 = new Timer(timedCommands[1].timer);
            timer.Start();
            timer2.Start();

            timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            timer2.Elapsed += new ElapsedEventHandler(timerElapsed);

            void _timer_Elapsed(object sender, ElapsedEventArgs e)
            {
                irc.WriteChatMessage(timedCommands[0].message);
            }

            void timerElapsed(object sender, ElapsedEventArgs e)
            {
                irc.WriteChatMessage(timedCommands[1].message);
            }

            PingSender ping = new PingSender(irc);
            ping.Start();

            while (true)
            {
                // read any message from the chat room
                string fullMessage = irc.ReadMessage();
                if(fullMessage.Contains("PRIVMSG"))
                {

                    string username = ChatBot.GetUsername(fullMessage);
                    string message = ChatBot.GetMessage(fullMessage);
                    User user = irc.getUser(username);
                    Subs subs = irc.getUserSubscriber(user);

                    var test = subs.subscriptions.FirstOrDefault(e => e.user.name.Contains(username));
                    //{
                    //    if(sub.user.name.Contains(username))
                    //    {
                    //        irc.WriteChatMessage("This user is a sub and allowed to post a message!");
                    //        break;
                    //    }
                    //    else
                    //    {
                    //        irc.WriteChatMessage("This user is not a sub");
                    //    }
                    //}
                    

                    bool link = ChatBot.checkLink(message);
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
                                foreach (Command commandd in commands)
                                {
                                    if(commands.Any(c => c.keyword == commandMessage) || kappamonCommands.Contains(commandMessage))
                                    {
                                        if (commandd.keyword == commandMessage)
                                        {
                                            DateTime testDate = new DateTime(1, 1, 1);
                                            DateTime date = DateTime.Now;
                                            if (commandd.startedTime == testDate)
                                            {
                                                commandd.startedTime = date;
                                                if (commandd.keyword == "commands")
                                                {
                                                    commandd.message += commandsText;
                                                }

                                                if (commandd.type == "request")
                                                {
                                                    string query = commandd.request;
                                                    mySqlConnection.Open();
                                                    MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlConnection);

                                                    if(commandd.request.Contains("SELECT"))
                                                    {
                                                        var result = mySqlCommand.ExecuteScalar();
                                                        mySqlConnection.Close();
                                                        if(result == null)
                                                        {
                                                            irc.WriteChatMessage("There was a problem executing that command");
                                                        }
                                                        else
                                                        {
                                                            if (commandd.message.Contains("@"))
                                                            {
                                                                irc.WriteChatMessage(commandd.message.Replace("@", result.ToString()));
                                                            }
                                                        }
                                                    }
                                                    else if(commandd.request.Contains("UPDATE"))
                                                    {
                                                        int result = mySqlCommand.ExecuteNonQuery();
                                                        mySqlConnection.Close();
                                                        if (result < 0)
                                                        {
                                                            
                                                        }
                                                        else
                                                        {
                                                            irc.WriteChatMessage(commandd.message);
                                                        }
                                                    }


                                                }
                                                else
                                                {
                                                    irc.WriteChatMessage(commandd.message);
                                                }
                                            }
                                            else
                                            {
                                                TimeSpan span = date - commandd.startedTime;
                                                int ms = (int)span.TotalMilliseconds;
                                                if (ms <= commandd.cooldown)
                                                {
                                                    irc.WriteChatMessage("This command is in cooldown right now, be patient !");
                                                }
                                                else
                                                {
                                                    irc.WriteChatMessage(commandd.message);
                                                    commandd.startedTime = DateTime.Now;
                                                }

                                            }
                                        }
                                    }
                                    else
                                    {
                                        irc.WriteChatMessage("This command does not exist, type !commands to know what commands are available");
                                        break;
                                    }
                                    
                                }
                            }
                        }
                        else
                        {
                            //
                        }
                    }
                }
            }
        }
    }



