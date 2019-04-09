using Moonbot_Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MoonBot_Data
{
    public static class FollowerD
    {
        public static void insertFollowers(int offset, string channelId)
        {
            ExampleA follower = new ExampleA();
            string url = "https://api.twitch.tv/kraken/channels/" + channelId + "/follows?offset=" + offset;
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            if (webRequest != null)
            {
                webRequest.Method = "GET";
                webRequest.Timeout = 12000;
                webRequest.ContentType = "application/json";
                webRequest.Accept = "Accept: application/vnd.twitchtv.v5+json";
                webRequest.Headers.Add("Client-ID", "v31jt30fnjpg8qgu0ucfbd3ly0q5tx");
                webRequest.Headers.Add("Authorization: OAuth 2tj232fx71a9jhd9hu61crlrj5nced");
            }

            try
            {
                using (Stream s = webRequest.GetResponse().GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(s))
                    {
                        var jsonResponse = sr.ReadToEnd();
                        follower = JsonConvert.DeserializeObject<ExampleA>(jsonResponse);
                        Console.WriteLine(jsonResponse);
                    }
                }

            }
            catch (Exception ex)
            {
            }

        }
    }
}
