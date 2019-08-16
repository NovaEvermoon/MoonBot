using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Moonbot_Objects;
using MoonBot_Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using Moonbot_Objects;

namespace MoonBot_Data
{
    public static class FollowerD
    {
        public static void InsertFollowers(ChannelO channel)
        {
            int offset = 0;
            List<Follow> followers = new List<Follow>();
            FollowerO followersO =  ChannelD.GetChannelFollowers(offset, channel);
            int total = followersO._total;

            

            while(offset < total)
            {
                foreach (Follow follow in followersO.follows)
                {
                    followers.Add(follow);
                    offset += 1;
                    
                }
                followersO = ChannelD.GetChannelFollowers(offset, channel);
            }


            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MysqlMoonBotDataBase"].ConnectionString;
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    using (MySqlCommand cmdUpdateUser = new MySqlCommand())
                    {
                        cmdUpdateUser.CommandText = "UpdateUser";
                        cmdUpdateUser.Connection = mySqlConnection;
                        cmdUpdateUser.CommandType = CommandType.StoredProcedure;
                        foreach(Follow follow in followers)
                        {
                            if (cmdUpdateUser.Parameters.Contains("twitchId"))
                                cmdUpdateUser.Parameters["twitchId"].Value = follow.user._id;
                            else
                                cmdUpdateUser.Parameters.AddWithValue("twitchId", follow.user._id);

                            if (cmdUpdateUser.Parameters.Contains("displayName"))
                                cmdUpdateUser.Parameters["displayName"].Value = follow.user.display_name;
                            else
                                cmdUpdateUser.Parameters.AddWithValue("displayName", follow.user.display_name);

                            if(cmdUpdateUser.Parameters.Contains("name"))
                                cmdUpdateUser.Parameters["name"].Value = follow.user.name;
                            else
                                cmdUpdateUser.Parameters.AddWithValue("name", follow.user.name);

                            MySqlDataReader result = cmdUpdateUser.ExecuteReader();
                            result.Close();
                                                      
                        }
                    }

                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "InsertFollower";
                        cmd.Connection = mySqlConnection;
                        cmd.CommandType = CommandType.StoredProcedure;
                        foreach (Follow follow in followers)
                        {
                            if (cmd.Parameters.Contains("createdAt"))
                                cmd.Parameters["createdAt"].Value = follow.created_at;
                            else
                                cmd.Parameters.AddWithValue("createdAt", follow.created_at);

                            if (cmd.Parameters.Contains("twitchId"))
                                cmd.Parameters["twitchId"].Value = follow.user._id;
                            else
                                cmd.Parameters.AddWithValue("twitchId", follow.user._id);

                            MySqlDataReader result = cmd.ExecuteReader();
                            result.Close();
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }

        public static string getFollowage(Dictionary<string,string> parameters)
        {

            string userName = parameters["username"];
            string channelId = parameters["channelId"];

            UserO user = UserD.GetUser(userName);

            string channelOauth = ConfigurationManager.AppSettings["channelOauth"];
            Followage followage = new Followage();
            StringBuilder message = new StringBuilder();

            string url = string.Format("https://api.twitch.tv/kraken/users/{0}/follows/channels/{1}", user.users[0]._id, channelId) ;
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            if (webRequest != null)
            {
                webRequest.Method = "GET";
                webRequest.Timeout = 12000;
                webRequest.Headers.Add("Client-ID", channelOauth);
                webRequest.ContentType = "application/json";
                webRequest.Accept = "application/vnd.twitchtv.v5+json";
            }

            using (Stream s = webRequest.GetResponse().GetResponseStream())
            {
                using (StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();
                    followage = JsonConvert.DeserializeObject<Followage>(jsonResponse);
                }
            }

            if (followage.created_at == null)
            {
                message.Append(String.Format("{0} is not following the channel, what are you even waiting for D:", user.users[0].display_name));
            }
            else
            {
                DateTime baseDate = new DateTime(1, 1, 1);
                DateTime FollowageDate = followage.created_at;
                DateTime currentDate = DateTime.Now.ToLocalTime();

                TimeSpan span = currentDate - FollowageDate;

                int years = (baseDate + span).Year - 1;
                int months = (baseDate + span).Month - 1;
                int days = (baseDate + span).Day;
                int hours = (baseDate + span).Hour;
                int minutes = (baseDate + span).Minute;
                int seconds = (baseDate + span).Second;

                message.Append(string.Format("{0} has been following the channel for ", user.users[0].display_name));

                if(years != 0)
                {
                    if(years >1)
                    {
                        message.Append(string.Format("{0} years ", years));
                    }
                    else
                    {
                        message.Append(string.Format("{0} year ", years));
                    }
                }

                if (months != 0)
                {
                    if (months > 1)
                    {
                        message.Append(string.Format("{0} months ", months));
                    }
                    else
                    {
                        message.Append(string.Format("{0} month ", months));
                    }
                }

                if (days != 0)
                {
                    if (days > 1)
                    {
                        message.Append(string.Format("{0} days ", days));
                    }
                    else
                    {
                        message.Append(string.Format("{0} day ", days));
                    }
                }

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

            message.Append(". Thank you so much for the amazing support! ( ˘ ³˘)♡ ");

            return message.ToString();
        }
    }
}
