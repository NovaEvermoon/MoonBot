using Moonbot_Objects.User;
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
        public static void insertUser(UserO user)
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
                        cmd.Parameters.AddWithValue("displayName", user.data[0].display_name);
                        cmd.Parameters.AddWithValue("twitchId", user.data[0].id);
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
        public static UserO getUser(string username)
        {

            string readUserToken = ConfigurationManager.AppSettings["userReadToken"];
            UserO user = new UserO();
            string url = "https://api.twitch.tv/helix/users?login=" + username;
            var webRequest = System.Net.WebRequest.Create(url);
            if (webRequest != null)
            {
                webRequest.Method = "GET";
                webRequest.Timeout = 12000;
                webRequest.ContentType = "application/json";
                webRequest.Headers.Add("Client-ID", readUserToken);

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
    }
}
