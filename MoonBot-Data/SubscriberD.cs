using Moonbot_Objects.User;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Excel = Microsoft.Office.Interop.Excel;

namespace MoonBot_Data
{
    public static class SubscriberD
    {
        /*public static void InsertSubsFromExcel()
        {
            List<Subscription> subs = new List<Subscription>();
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"C:\Users\Nova\Documents\Subs.xlsx");
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            int r = xlRange.Rows.Count;
            int c = xlRange.Columns.Count;
            for (int i = 1; i <= r; i++)
            {
                Subscription sub = new Subscription();
                UserO user = UserD.getUser(xlRange.Cells[i, 2].Value2.ToString());
                if(user.users.Count > 0)
                    sub._id = user.users[0]._id;
                    sub.created_at = Convert.ToDateTime(xlRange.Cells[i, 1].Value.ToString());
                    sub.sub_plan = xlRange.Cells[i, 3].Value2.ToString();
                    subs.Add(sub);
            }

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MysqlMoonBotDataBase"].ConnectionString;
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "InsertSubscriber";
                        cmd.Connection = mySqlConnection;
                        cmd.CommandType = CommandType.StoredProcedure;
                        foreach (Subscription sub in subs)
                        {
                            if (cmd.Parameters.Contains("twitchId"))
                                cmd.Parameters["twitchId"].Value = sub._id;
                            else
                                cmd.Parameters.AddWithValue("twitchId", sub._id);

                            if (cmd.Parameters.Contains("createdAt"))
                                cmd.Parameters["createdAt"].Value = sub.created_at ;
                            else
                                cmd.Parameters.AddWithValue("createdAt", sub.created_at);

                            if (cmd.Parameters.Contains("tier"))
                                cmd.Parameters["tier"].Value = sub.sub_plan;
                            else
                                cmd.Parameters.AddWithValue("tier", sub.sub_plan);

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
        }*/
    }
}
