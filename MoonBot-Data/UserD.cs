using Moonbot_Objects;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Stream = System.IO.Stream;

namespace MoonBot_Data
{
    public static class UserD
    {
        public static void InsertUser(UserO user)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MysqlMoonBotDataBase"].ConnectionString;
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "InsertUser";
                        cmd.Connection = mySqlConnection;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("displayName", user.users[0].display_name);
                        cmd.Parameters.AddWithValue("twitchId", user.users[0]._id);
                        var test = cmd.ExecuteReader();
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public static string GetUsername(string fullMessage)
        {
            string username;
            int intIndexParseSign = fullMessage.IndexOf(':');
            if (fullMessage.Contains('!'))
            {
                int indexNicknameEnd = fullMessage.IndexOf('!');
                username = fullMessage.Substring(1, indexNicknameEnd - 1);
            }
            else
            {
                username = "";
            }

            return username;
        }
        public static UserO GetUser(string username)
        {
            string channelOauth = ConfigurationManager.AppSettings["channelOauth"];
            string readUserToken = ConfigurationManager.AppSettings["userReadToken"];
            UserO user = new UserO();
            string url = "https://api.twitch.tv/kraken/users?login=" + username;
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            if (webRequest != null)
            {
                webRequest.Method = "GET";
                webRequest.Timeout = 12000;
                webRequest.Headers.Add("Client-ID", channelOauth);
                webRequest.ContentType = "application/json";
                webRequest.Accept = "application/vnd.twitchtv.v5+json";
                webRequest.Headers.Add("Authorization: " + readUserToken);
            }

            using (Stream s = webRequest.GetResponse().GetResponseStream())
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();
                    user = JsonConvert.DeserializeObject<UserO>(jsonResponse);
                }
            }

            return user;
        }
        public static int GetUserShard(string username)
        {
            int shards = 23;

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MysqlMoonBotDataBase"].ConnectionString;
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    string query = string.Format("SELECT user_shards FROM user WHERE user_name='{0}'", username);

                    MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlConnection);
                    shards = Convert.ToInt32(mySqlCommand.ExecuteScalar());

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return shards;
        }
        public static UserO PermitUser(Dictionary<string, dynamic> parameters)
        {
            UserO user = (UserO)parameters["_user"];
            user.isPermit = true;
            CustomTimer timer = new CustomTimer(120000, user);
            timer.Start();
            timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);

            return user;
        }

        public static void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (sender != null)
            {
                ((CustomTimer)sender).user.isPermit = false;
            }
        }
    }
}
