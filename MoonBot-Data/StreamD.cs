using Moonbot_Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Stream = System.IO.Stream;

namespace MoonBot_Data
{
    public static class StreamD
    {
        public static StreamO GetStreamByUser(string channelId)
        {
            StreamO stream = new StreamO();

            string channelOauth = ConfigurationManager.AppSettings["channelOauth"];
            string helixChannelSubscriptionToken = ConfigurationManager.AppSettings["helixChannelSubscriptionToken"];
            string url = "https://api.twitch.tv/kraken/streams/" + channelId;

            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                if (webRequest != null)
                {
                    webRequest.Method = "GET";
                    webRequest.Timeout = 12000;
                    webRequest.Headers.Add("Client-ID", channelOauth);
                    webRequest.ContentType = "application/json";
                    webRequest.Accept = "application/vnd.twitchtv.v5+json";
                    webRequest.Headers.Add("Authorization: " + helixChannelSubscriptionToken);
                }

                using (Stream s = webRequest.GetResponse().GetResponseStream())
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                    {
                        var jsonResponse = sr.ReadToEnd();
                        stream = JsonConvert.DeserializeObject<StreamO>(jsonResponse);
                    }
                }
            }
            catch(Exception ex)
            {
                StringBuilder sb = new StringBuilder(DateTime.Now.ToString("dd-MM-yyyy") + " : " + ex.Message);
                Console.WriteLine(sb);
            }
            

            return stream;
        }

        public static string GetStreamUptime(Dictionary<string, string> parameters)
        {
            DateTime uptime = Convert.ToDateTime(parameters["_streamUptime"]);

            StringBuilder message = new StringBuilder();

            try
            {
                if (uptime == Convert.ToDateTime("1/1/00001"))
                {
                    message.Append("The streamer is offline right now, come back later~");
                }
                else
                {
                    message.Append("Nova has been live for ");

                    DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, uptime.Hour, uptime.Minute, uptime.Second);

                    DateTime baseDate = new DateTime(1, 1, 1);
                    DateTime currentDate = DateTime.Now.ToLocalTime();

                    TimeSpan span = currentDate - uptime;

                    int hours = (baseDate + span).Hour;
                    int minutes = (baseDate + span).Minute;
                    int seconds = (baseDate + span).Second;

                    if (hours != 0)
                    {
                        if (hours > 1)
                        {
                            message.Append(string.Format("{0} hours ", hours));
                        }
                        else
                        {
                            message.Append(string.Format("{0} hour ", hours));
                        }
                    }

                    if (minutes != 0)
                    {
                        if (minutes > 1)
                        {
                            message.Append(string.Format("{0} minutes ", minutes));
                        }
                        else
                        {
                            message.Append(string.Format("{0} minute ", minutes));
                        }
                    }

                    if (seconds != 0)
                    {
                        if (seconds > 1)
                        {
                            message.Append(string.Format("{0} seconds ", seconds));
                        }
                        else
                        {
                            message.Append(string.Format("{0} second ", seconds));
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                StringBuilder sb = new StringBuilder(DateTime.Now.ToString("dd-MM-yyyy") + " : " + ex.Message);
                Console.WriteLine(sb);
            }
            return message.ToString() ;
        }
    }
}
