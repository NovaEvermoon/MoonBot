using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Moonbot_Objects.Channel;
using MoonBot_Data;
using Moonbot_Objects.User;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace MoonBot_Data
{
    public static class FollowerD
    {
        public static void insertFollowers(ChannelO channel)
        {
            int offset = 0;
            List<Follow> followers = new List<Follow>();
            FollowerO followersO =  ChannelD.getChannelFollowers(offset, channel);
            int total = followersO._total;

            

            while(offset < total)
            {
                foreach (Follow follow in followersO.follows)
                {
                    followers.Add(follow);
                    offset += 1;
                    
                }
                followersO = ChannelD.getChannelFollowers(offset, channel);
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
    }
}
