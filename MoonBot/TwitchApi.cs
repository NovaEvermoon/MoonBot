﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MoonBot
{
    public class TwitchApi
    {
        public  JsonFollowersAnswer GetFollowersAnswer(string channelId, string clientId, int count, string cursor)
        {
            string url = "";
            JsonFollowersAnswer jsonFowllowAnswer = new JsonFollowersAnswer();

            if (count > 0)
            {
                url = @"https://api.twitch.tv/helix/users/follows?to_id=" + channelId + "&after=" + cursor;
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
        public Subscription getUserSubscriber(User user)
        {
            Subscription sub = new Subscription();
            string id = "";
            foreach (Data data in user.data)
            {
                id = data.id;
            }

            //string url = "https://api.twitch.tv/kraken/channels/" + ChatBot.channelId + "/subscriptions?offset=0";
            string url = "https://api.twitch.tv/kraken/channels/" + ChatBot.channelId + "/subscriptions/" + id;

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            if (webRequest != null)
            {
                webRequest.Method = "GET";
                webRequest.Timeout = 12000;
                webRequest.ContentType = "application/json";
                webRequest.Accept = "Accept: application/vnd.twitchtv.v5+json";
                webRequest.Headers.Add("Client-ID", ChatBot.clientID);
                webRequest.Headers.Add("Authorization: OAuth 2tj232fx71a9jhd9hu61crlrj5nced");
            }

            try
            {
                using (Stream s = webRequest.GetResponse().GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(s))
                    {
                        var jsonResponse = sr.ReadToEnd();

                        Console.WriteLine(jsonResponse);
                        sub = JsonConvert.DeserializeObject<Subscription>(jsonResponse);
                    }
                }

            }
            catch (Exception ex)
            {
                sub = new Subscription();
            }

            return sub;

        }

        public Follower GetUserFollower(User user)
        {
            Follower follower = new Follower();
            string url = "https://api.twitch.tv/kraken/users/"+user.data[0].id+"/follows/channels/"+ChatBot.channelId;
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            if (webRequest != null)
            {
                webRequest.Method = "GET";
                webRequest.Timeout = 12000;
                webRequest.ContentType = "application/json";
                webRequest.Accept = "Accept: application/vnd.twitchtv.v5+json";
                webRequest.Headers.Add("Client-ID", ChatBot.clientID);
                webRequest.Headers.Add("Authorization: OAuth 2tj232fx71a9jhd9hu61crlrj5nced");
            }

            try
            {
                using (Stream s = webRequest.GetResponse().GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(s))
                    {
                        var jsonResponse = sr.ReadToEnd();

                        Console.WriteLine(jsonResponse);
                        follower = JsonConvert.DeserializeObject<Follower>(jsonResponse);
                    }
                }

            }
            catch (Exception ex)
            {
            }

            return follower;
        }

        public string getFollowage(Follower follower, string userName)
        {
            DateTime today = DateTime.Now;
            DateTime zeroTime = new DateTime(1, 1, 1);

            TimeSpan span = today - follower.created_at;


            int years = (zeroTime + span).Year - 1;
            int months = (zeroTime + span).Month - 1;
            int days = (zeroTime + span).Day;

            string message = userName + " has been following the channel for ";
            if(years >0 && years < 2)
            {
                message += years + " year ";
            }
            else if (years >= 2)
            {
                message += years + " years ";
            }

            if (months > 0 && months < 2)
            {
                message += months + " month ";
            }
            else if (months >= 2)
            {
                message += months + " months ";
            }

            if (days > 0 && days < 2)
            {
                message += days + " day ";
            }
            else if (days >= 2)
            {
                message += days + " days ";
            }

            return message;

        }

    }
}
