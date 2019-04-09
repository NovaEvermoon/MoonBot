using Moonbot_Objects;
using Moonbot_Objects.TwitchJsonAnswer;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoonBot_Data
{
    public static class UserD
    {
        public static void insertUser(Moonbot_Objects.User user)
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
                    cmd.Parameters.AddWithValue("@displayName", user.display_name);
                    cmd.Parameters.AddWithValue("@name", user.name);
                    cmd.Parameters.AddWithValue("@twitchId", user._id);
                    var test = cmd.ExecuteReader();
                }

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
        public static Datum getUser(string username)
        {
            Datum user = new Datum();
            string url = "https://api.twitch.tv/helix/users?login=" + username;
            var webRequest = System.Net.WebRequest.Create(url);
            if (webRequest != null)
            {
                webRequest.Method = "GET";
                webRequest.Timeout = 12000;
                webRequest.ContentType = "application/json";
                webRequest.Headers.Add("Client-ID", "v31jt30fnjpg8qgu0ucfbd3ly0q5tx");

            }

            using (Stream s = webRequest.GetResponse().GetResponseStream())
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();
                    object test = JsonConvert.DeserializeObject<Moonbot_Objects.TwitchJsonAnswer.User>(jsonResponse);
                    user = JsonConvert.DeserializeObject<Datum>(jsonResponse);
                }
            }

            return user;
        }
    }
}
