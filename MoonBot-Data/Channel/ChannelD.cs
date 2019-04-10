﻿using Moonbot_Objects.Channel;
using Moonbot_Objects.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MoonBot_Data.Channel
{
    public static class ChannelD
    {
        public static ChannelO getChannel()
        {
            ChannelO channel = new ChannelO();
            string channelOauth = ConfigurationManager.AppSettings["channelOauth"];
            string readChannelOauth = ConfigurationManager.AppSettings["channelReadToken"];
            string url = "https://api.twitch.tv/kraken/channel";
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            if (webRequest != null)
            {
                webRequest.Method = "GET";
                webRequest.Timeout = 12000;
                webRequest.ContentType = "application/json";
                webRequest.Accept = "Accept: application/vnd.twitchtv.v5+json";
                webRequest.Headers.Add("Client-ID", channelOauth);
                webRequest.Headers.Add("Authorization: "+ readChannelOauth);
            }

            try
            {
                using (Stream s = webRequest.GetResponse().GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(s))
                    {
                        var jsonResponse = sr.ReadToEnd();
                        channel = JsonConvert.DeserializeObject<ChannelO>(jsonResponse);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return channel;
        }
        public static FollowerO getChannelFollowers(int offset,ChannelO channel)
        {
            FollowerO followers = new FollowerO();
            string channelOauth = ConfigurationManager.AppSettings["channelOauth"];
            string readChannelOauth = ConfigurationManager.AppSettings["channelReadToken"];
            string url = "https://api.twitch.tv/kraken/channels/"+channel._id+"/follows?offset="+offset;
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            if (webRequest != null)
            {
                webRequest.Method = "GET";
                webRequest.Timeout = 12000;
                webRequest.ContentType = "application/json";
                webRequest.Accept = "Accept: application/vnd.twitchtv.v5+json";
                webRequest.Headers.Add("Client-ID", channelOauth);
                webRequest.Headers.Add("Authorization: " + readChannelOauth);
            }

            try
            {
                using (Stream s = webRequest.GetResponse().GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(s))
                    {
                        var jsonResponse = sr.ReadToEnd();
                        followers = JsonConvert.DeserializeObject<FollowerO>(jsonResponse);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return followers;
        }
        public static string getChannelTitle(ChannelO channel)
        {
            string title = "The stream's current title is : " + channel.status;
            return title;
        }
        public static string getChannelGame(ChannelO channel)
        {
            string game = "Currently playing : " + channel.game;
            return game;
        }
        public static void udateChannelTitle(ChannelO channel)
        {
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.twitch.tv/kraken/channels/" + ChatBot.channelId);
        //    request.Method = "PUT";
        //    string postData = "This is a test that posts this string to a Web server.";
        //    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
        //    //request.Headers.Add("content-type", "application/json");
        //    request.ContentType = "application/json";
        //    request.Headers.Add("cache-control", "no-cache");
        //    // Set the ContentLength property of the WebRequest.
        //    request.ContentLength = byteArray.Length;
        //    //request.Headers.Add("content-length", byteArray.Length.ToString());
        //    Stream dataStream = request.GetRequestStream();
        //    dataStream.Write(byteArray, 0, byteArray.Length);
        //    dataStream.Close();
        //    WebResponse response = request.GetResponse();
        //    Console.WriteLine(((HttpWebResponse)response).StatusDescription);
        //    dataStream = response.GetResponseStream();
        //    StreamReader reader = new StreamReader(dataStream);
        //    string responseFromServer = reader.ReadToEnd();
        //    Console.WriteLine(responseFromServer);
        //    // Clean up the streams.
        //    reader.Close();
        //    dataStream.Close();
        //    response.Close();




        //    //HttpWebRequest wrequest = (HttpWebRequest)WebRequest.Create("https://api.twitch.tv/kraken/channels/" + ChatBot.channelId);
        //    //wrequest.Method = "PUT";
        //    //wrequest.Headers.Add("cache-control", "no-cache");
        //    //wrequest.Headers.Add("content-type", "application/json");
        //    //wrequest.Headers.Add("Client-ID", ChatBot.clientID);
        //    //wrequest.Headers.Add("authorization", "OAuth yiha3wvz45tm8daeajmcijwh281u2b");
        //    //wrequest.Headers.Add("accept", "application/vnd.twitchtv.v3+json");
        //    //wrequest.ContentType = "application/xml";

        //    //channel.status = "TEST";
        //    //if (channel != null)
        //    //{
        //    //    wrequest.ContentLength = 500;
        //    //    Stream dataStream = wrequest.GetRequestStream();
        //    //    Serialize(dataStream, channel);
        //    //    dataStream.Close();
        //    //}

        //    //HttpWebResponse wresponse = (HttpWebResponse)wrequest.GetResponse();
        //    //string returnString = wresponse.StatusCode.ToString();

        }
    }

}