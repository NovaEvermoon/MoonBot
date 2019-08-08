using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moonbot_Objects.Command;
using MySql.Data.MySqlClient;

namespace MoonBot_Data
{
    public static class CommandD
    {
        public static List<CommandO> loadCommands()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MysqlMoonBotDataBase"].ConnectionString;
            List<CommandO> commands = new List<CommandO>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();
                    string query = "SELECT * FROM command WHERE command_status != 0";
                    MySqlCommand command = new MySqlCommand(query, mySqlConnection);
                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            CommandO chatCommand = new CommandO();
                            try
                            {
                                chatCommand.id = reader.GetInt32(0);
                                chatCommand.keyword = reader.GetString(1);
                                chatCommand.message = reader.GetString(2);
                                chatCommand.userLevel = reader.GetString(3);
                                chatCommand.cooldown = reader.GetInt32(4);
                                chatCommand.status = reader.GetBoolean(5);
                                chatCommand.timer = reader.GetInt32(6);
                                chatCommand.description = reader.GetString(7);
                                chatCommand.type = reader.GetString(8);
                                chatCommand.request = reader.GetString(9);
                                chatCommand.parameter = reader.GetString(10);
                                chatCommand.file = reader.GetString(11);
                                chatCommand.condition = reader.GetString(12);
                                commands.Add(chatCommand);
                            }
                            catch (Exception ex)
                            {
                                StringBuilder sb = new StringBuilder(DateTime.Now.ToString("dd-MM-yyyy") + " : " + ex.Message);
                                Console.WriteLine(sb);
                            }

                        }
                    }
                }
            }
            catch(Exception ex)
            {
                StringBuilder sb = new StringBuilder(DateTime.Now.ToString("dd-MM-yyyy") + " : " + ex.Message);
                Console.WriteLine(sb);
            }
            
            return commands;
        }
        public static bool isKappamonCommand(string commandMessage)
        {
            string[] kappamonCommands = { "song", "feed", "meow" };
            bool exists = false;
            
            if(kappamonCommands.Contains(commandMessage))
            {
                exists = true;
            }
            return exists;
        }
        public static Tuple<int,string> executeSelectCommand(string query)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MysqlMoonBotDataBase"].ConnectionString;
            Tuple<int, string> answer = null;
            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
            {
                mySqlConnection.Open();
                MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlConnection);
                var result = mySqlCommand.ExecuteScalar();

                mySqlConnection.Close();
                if (result == null)
                {
                    answer = new Tuple<int, string>(0,"There was a problem executing that command");
                }
                else
                {
                    answer = new Tuple<int, string>(Convert.ToInt32(result), "");
                }

            }

            return answer;
        }
        public static Tuple<int, string> executeUpdateCommand(string query, string message)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MysqlMoonBotDataBase"].ConnectionString;
            Tuple<int, string> answer = null;

            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
            {
                mySqlConnection.Open();
                MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlConnection);
                int result = mySqlCommand.ExecuteNonQuery();
                mySqlConnection.Close();
                if (result < 0)
                {
                    answer = new Tuple<int, string>(result, "There was an error executing this request");
                }
                else
                {
                    answer = new Tuple<int, string>(result, message);
                }
            }

            return answer;
        }

        public static string getCommandUser(string message)
        {
            string user = "";

            return user;
        }
    }
}
