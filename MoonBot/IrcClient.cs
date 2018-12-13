using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace MoonBot
{
    public class IrcClient
    {

        public string botName;
        private string channelName;

        private TcpClient tcpClient;
        private StreamReader ircReader;
        private StreamWriter writer;

        public IrcClient(string Ip, int Port, string BotName, string Password, string ChannelName)
        {
            try
            {
                this.botName = BotName;
                this.channelName = ChannelName;

                tcpClient = new TcpClient(Ip, Port);
                ircReader = new StreamReader(tcpClient.GetStream());
                writer = new StreamWriter(tcpClient.GetStream());

                // Try to join the room
                writer.WriteLine("PASS " + Password);
                writer.WriteLine("NICK " + BotName);
                writer.WriteLine("USER " + BotName + " 8 * :" + BotName);
                writer.WriteLine("JOIN #" + ChannelName);
                writer.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void WriteConsoleMessage(string Message)
        {
            try
            {
                writer.WriteLine(Message);
                writer.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void WriteChatMessage(string message)
        {
            try
            {
                WriteConsoleMessage(":" + botName + "!" + botName + "@" + botName +
                ".tmi.twitch.tv PRIVMSG #" + channelName + " :" + message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public string ReadMessage()
        {
            try
            {
                string message = ircReader.ReadLine();

                return message;
            }
            catch (Exception ex)
            {
                return "Error receiving message: " + ex.Message;
            }
        }

        public JsonFollowersAnswer GetFollowersAnswer(string channelId, string clientId, int count, string cursor)
        {
            string url = "";
            JsonFollowersAnswer jsonFowllowAnswer = new JsonFollowersAnswer();

            if (count > 0)
            {
                url = @"https://api.twitch.tv/helix/users/follows?to_id=" + channelId +"&after=" + cursor  ;
            }
            else
            {
                url = @"https://api.twitch.tv/helix/users/follows?to_id=" + channelId;
            }
            

            var webRequest = System.Net.WebRequest.Create(url);
            if (webRequest != null)
            {
                webRequest.Method = "GET";
                webRequest.Timeout = 12000;
                webRequest.ContentType = "application/json";
                webRequest.Headers.Add("Client-ID", clientId);

            }

            using (Stream s = webRequest.GetResponse().GetResponseStream())
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();
                    Console.WriteLine(jsonResponse);
                     jsonFowllowAnswer = JsonConvert.DeserializeObject<JsonFollowersAnswer>(jsonResponse);
                }
            }
            return jsonFowllowAnswer;
        }

        public User getUser(string username)
        {
            User user = new User();
            string url = "https://api.twitch.tv/helix/users?login=" + username;
            var webRequest = System.Net.WebRequest.Create(url);
            if (webRequest != null)
            {
                webRequest.Method = "GET";
                webRequest.Timeout = 12000;
                webRequest.ContentType = "application/json";
                webRequest.Headers.Add("Client-ID", ChatBot.clientID);

            }

            using (Stream s = webRequest.GetResponse().GetResponseStream())
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd(); 
                    
                    Console.WriteLine(jsonResponse);
                    user = JsonConvert.DeserializeObject<User>(jsonResponse);
                }
            }

            return user;
        }

        public Subs getUserSubscriber(User user)
        {
            Subs subs = new Subs();
            string id = "";
            foreach(Data data in user.data)
            {
                id = data.id;
            }

            string url = "https://api.twitch.tv/kraken/channels/" + ChatBot.channelId + "/subscriptions";
            //string url = "https://api.twitch.tv/kraken/users/" + id + "/subscriptions/"+ ChatBot.channelId ;

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            if (webRequest != null)
            {
                webRequest.Method = "GET";
                webRequest.Timeout = 12000;
                webRequest.ContentType = "application/json";
                webRequest.Accept = "Accept: application/vnd.twitchtv.v5+json";
                webRequest.Headers.Add("Client-ID", ChatBot.clientID);
                webRequest.Headers.Add("Authorization: OAuth l3iwchpk1lqyv69zp9mwgg8hjiyqrq");
            }

            using (Stream s = webRequest.GetResponse().GetResponseStream())
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();

                    Console.WriteLine(jsonResponse);
                    subs = JsonConvert.DeserializeObject<Subs>(jsonResponse);
                }
            }
            return subs;
        }
    }


}
