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
            IrcClient irc = new IrcClient("irc.twitch.tv", 6667, ChatBot.botName, ChatBot.password, ChatBot.broadcasterName);

            PingSender ping = new PingSender(irc);
            ping.Start();

            while (true)
            {
                // read any message from the chat room
                string message = irc.ReadMessage();


                //string url = @"https://api.twitch.tv/helix/users?login=novaevermoon&client_id=" + ChatBot.clientID;
                //string url = @"https://api.twitch.tv/helix/users?login=terror_seeds&client_id=" + ChatBot.clientID;
                string url = @"https://api.twitch.tv/helix/users/follows?to_id=167461349";
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
