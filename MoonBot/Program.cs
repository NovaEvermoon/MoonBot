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
            
            while(reader.Read())
            {
                Command commandTest = new Command();
                commandTest.id = reader.GetInt32(0);
                commandTest.keyword = reader.GetString(1);
                commandTest.message = reader.GetString(2);
                commandTest.userLevel = reader.GetString(3);
                commandTest.cooldown = reader.GetInt32(4);
                commandTest.status = reader.GetBoolean(5);
                commandTest.description = reader.GetString(6);

                commands.Add(commandTest);
            }
            
            IrcClient irc = new IrcClient("irc.twitch.tv", 6667, ChatBot.botName, ChatBot.password, ChatBot.broadcasterName);

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



  


                //irc.WriteChatMessage("this is a test");

                //string url = @"https://api.twitch.tv/helix/users?login=novaevermoon&client_id=" + ChatBot.clientID;
                //string url = @"https://api.twitch.tv/helix/users?login=terror_seeds&client_id=" + ChatBot.clientID;
                string url = @"https://api.twitch.tv/helix/users/follows?to_id=167461349";

                //string getChannelUrl = @"https://api.twitch.tv/kraken/channels/ " +ChatBot.password;
                // //string getChannelUrl = @"https://api.twitch.tv/kraken/channels/" + ChatBot.broadcasterName;
                ////string getChannelUrl = @"https://api.twitch.tv/kraken/users?login=" + ChatBot.broadcasterName;
                ////string getChannelUrl = @"https://api.twitch.tv/kraken/chat/"+ ChatBot.channelID /* ChatBot.broadcasterName*/ +"/rooms";

                ////ChatBot.channelID ;
                //WebRequest getChannelWebRequest = WebRequest.Create(getChannelUrl);
                //if (getChannelWebRequest!=null)
                //{
                //    getChannelWebRequest.Method = "GET";
                //    getChannelWebRequest.Timeout = 12000;
                //    getChannelWebRequest.ContentType = "application/json";
                //    getChannelWebRequest.Headers.Add("Client-ID", ChatBot.clientID);
                //}

                //using (Stream s = getChannelWebRequest.GetResponse().GetResponseStream())
                //{
                //    using (StreamReader sr = new System.IO.StreamReader(s))
                //    {
                //        var jsonResponse = sr.ReadToEnd();
                //    }
                //}


                var webRequest = System.Net.WebRequest.Create(url);
                if (webRequest != null)
                {
                    webRequest.Method = "GET";
                    webRequest.Timeout = 12000;
                    webRequest.ContentType = "application/json";
                    webRequest.Headers.Add("Client-ID", ChatBot.clientID);

                }

                using (System.IO.Stream s = webRequest.GetResponse().GetResponseStream())
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                    {
                        var jsonResponse = sr.ReadToEnd();
                        User twitchUser = JsonConvert.DeserializeObject<User>(jsonResponse);
                        Console.WriteLine(String.Format("Response: {0}", jsonResponse));
                    }
                }

                if (message.Contains("PRIVMSG"))
                {
                    // messages from the users will look something like this (without quotes):
                    // format: ":[user]![user]@[user].tmi.twitch.tv privmsg #[channel] :[message]"

                    // modify message to only retrieve user and message
                    int intindexparsesign = message.IndexOf('!');
                    string username = message.Substring(1, intindexparsesign - 1); // parse username from specific section (without quotes)
                                                                                   // format: ":[user]!"
                                                                                   // get user's message
                    intindexparsesign = message.IndexOf(" :");
                    message = message.Substring(intindexparsesign + 2);

                }
            }
        }
    }
}
