using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MoonBot
{
    class Program
    {
        //TcpClient tcpClient;
        //StreamReader reader;
        //StreamWriter writer;

        static void Main(string[] args)
        {
            // Connection String.
            String connString = "Server=127.0.0.1;Database=MoonBot;port=3306;User Id=root;password=";

            MySqlConnection conn = new MySqlConnection(connString);
            conn.Open();
            string request = "SELECT * FROM command";
            MySqlCommand command = new MySqlCommand(request,conn);
            MySqlDataReader reader = command.ExecuteReader();

            List<Command> commands = new List<Command>();
            List<Command> timedCommands = new List<Command>();
            while(reader.Read())
            {
                Command commandTest = new Command();

                commandTest.id = reader.GetInt32(0);
                commandTest.keyword = reader.GetString(1);
                commandTest.message = reader.GetString(2);
                commandTest.userLevel = reader.GetString(3);
                commandTest.cooldown = reader.GetInt32(4);
                commandTest.status = reader.GetBoolean(5);
                commandTest.timer = reader.GetInt32(6);
                commandTest.description = reader.GetString(7);

                if(commandTest.timer == 0)
                {
                    commands.Add(commandTest);
                }
                else
                {
                    timedCommands.Add(commandTest);
                }
                
            }




            IrcClient irc = new IrcClient("irc.twitch.tv", 6667, ChatBot.botName, ChatBot.password, ChatBot.broadcasterName);

            JsonFollowersAnswer test = new JsonFollowersAnswer();
            int count = 0;

            test = irc.GetFollowersAnswer(ChatBot.channelId, ChatBot.clientID, count, "");

            count = test.data.Count();

            test = irc.GetFollowersAnswer(ChatBot.channelId, ChatBot.clientID, count, test.pagination.cursor);

            


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
                string message = irc.ReadMessage();

                char firstCharacter = message[0];
                if(firstCharacter == '!')
                {
                    string commandMessage = message.Substring(message.IndexOf('!') + 1);
                    if (commandMessage.Contains(" "))
                    {
                        string fullcommand = commandMessage.Substring(message.IndexOf(' '));
                        //command = fullcommand[0];
                        //var soUser = fullcommand[1];
                    }
                    else
                    {
                        foreach(Command commandd in commands)
                        {
                            if(commandd.keyword == commandMessage)
                            {
                                irc.WriteChatMessage(commandd.message);
                            }
                        }
                    }
                }

                

                string username = ChatBot.GetUsername(irc.ReadMessage());

                if(username == "novaevermoon" )
                {
                    
                }
                
                if (message.Contains("PRIVMSG"))
                {
                    int intindexparsesign = message.IndexOf('!');
                     username = message.Substring(1, intindexparsesign - 1); 
                                                                                                                                                  
                    intindexparsesign = message.IndexOf(" :");
                    message = message.Substring(intindexparsesign + 2);

                }
            }
        }
    }
}
