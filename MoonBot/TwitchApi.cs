﻿using Moonbot_Objects;
using Newtonsoft.Json;
using RestSharp;
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
        //public Subscription getUserSubscriber(User user)
        //{
        //    Subscription sub = new Subscription();
        //    string id = "";
        //    foreach (Data data in user.data)
        //    {
        //        id = data.id;
        //    }

        //    //string url = "https://api.twitch.tv/kraken/channels/" + ChatBot.channelId + "/subscriptions?offset=0";
        //    string url = "https://api.twitch.tv/kraken/channels/" + ChatBot.channelId + "/subscriptions/" + id;

        //    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
        //    if (webRequest != null)
        //    {
        //        webRequest.Method = "GET";
        //        webRequest.Timeout = 12000;
        //        webRequest.ContentType = "application/json";
        //        webRequest.Accept = "Accept: application/vnd.twitchtv.v5+json";
        //        webRequest.Headers.Add("Client-ID", ChatBot.clientID);
        //        webRequest.Headers.Add("Authorization: OAuth 2tj232fx71a9jhd9hu61crlrj5nced");
        //    }

        //    try
        //    {
        //        using (Stream s = webRequest.GetResponse().GetResponseStream())
        //        {
        //            using (StreamReader sr = new StreamReader(s))
        //            {
        //                var jsonResponse = sr.ReadToEnd();

        //                //Console.WriteLine(jsonResponse);
        //                sub = JsonConvert.DeserializeObject<Subscription>(jsonResponse);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        sub = new Subscription();
        //    }

        //    return sub;

        //}

        

        public Follower GetUserFollower(User user)
        {
            Follower follower = new Follower();
            string url = "";//"https://api.twitch.tv/kraken/users/"+user.data[0].id+"/follows/channels/"+ChatBot.channelId;
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

                        //Console.WriteLine(jsonResponse);
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

        public string GetTeamMember(string userName, string teamName)
        {
            string teamMember = "";
            Team team = new Team();
            team = GetTeamMembers(teamName);

            if(team.users.Any(b => b.name == userName))
            {
                teamMember = "IceWalker";
            }

            return teamMember;
        }

        public Team GetTeamMembers(string teamName)
        {
            Team team = new Team();

            string url = "https://api.twitch.tv/kraken/teams/" + teamName;
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

                        //Console.WriteLine(jsonResponse);
                        team = JsonConvert.DeserializeObject<Team>(jsonResponse);
                    }
                }

            }
            catch (Exception ex)
            {
            }

            return team;
        }

        public Channel getChannel()
        {

            Channel channel = new Channel();
            string url = "https://api.twitch.tv/kraken/channel";
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            if (webRequest != null)
            {
                webRequest.Method = "GET";
                webRequest.Timeout = 12000;
                webRequest.ContentType = "application/json";
                webRequest.Accept = "Accept: application/vnd.twitchtv.v5+json";
                webRequest.Headers.Add("Client-ID", ChatBot.clientID);
                webRequest.Headers.Add("Authorization: OAuth anbv0yz4999janso5ocxkaakd52yma");
            }

            try
            {
                using (Stream s = webRequest.GetResponse().GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(s))
                    {
                        var jsonResponse = sr.ReadToEnd();

                        //Console.WriteLine(jsonResponse);
                        channel = JsonConvert.DeserializeObject<Channel>(jsonResponse);
                    }
                }

            }
            catch (Exception ex)
            {
            }

            return channel;
        }

        public string getChannelTitle()
        {
            Channel channel = getChannel();
            string title = "The stream's current title is : " + channel.status;
            return title;
        }

        public string getChannelGame()
        {
            Channel channel = getChannel();
            string game = "Currently playing : " + channel.game;
            return game;
        }

        public void udateChannelTitle(Channel channel)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.twitch.tv/kraken/channels/" + ChatBot.channelId);
            request.Method = "PUT";
            string postData = "This is a test that posts this string to a Web server.";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            //request.Headers.Add("content-type", "application/json");
            request.ContentType = "application/json";
            request.Headers.Add("cache-control", "no-cache");
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;
            //request.Headers.Add("content-length", byteArray.Length.ToString());
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            Console.WriteLine(responseFromServer);
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();




            //HttpWebRequest wrequest = (HttpWebRequest)WebRequest.Create("https://api.twitch.tv/kraken/channels/" + ChatBot.channelId);
            //wrequest.Method = "PUT";
            //wrequest.Headers.Add("cache-control", "no-cache");
            //wrequest.Headers.Add("content-type", "application/json");
            //wrequest.Headers.Add("Client-ID", ChatBot.clientID);
            //wrequest.Headers.Add("authorization", "OAuth yiha3wvz45tm8daeajmcijwh281u2b");
            //wrequest.Headers.Add("accept", "application/vnd.twitchtv.v3+json");
            //wrequest.ContentType = "application/xml";

            //channel.status = "TEST";
            //if (channel != null)
            //{
            //    wrequest.ContentLength = 500;
            //    Stream dataStream = wrequest.GetRequestStream();
            //    Serialize(dataStream, channel);
            //    dataStream.Close();
            //}

            //HttpWebResponse wresponse = (HttpWebResponse)wrequest.GetResponse();
            //string returnString = wresponse.StatusCode.ToString();

        }

        public void Serialize(Stream output, object input)
        {
            //var ser = new DataContractSerializer(input.GetType());
            var test =  JsonConvert.SerializeObject(input);

        }

    }

}

