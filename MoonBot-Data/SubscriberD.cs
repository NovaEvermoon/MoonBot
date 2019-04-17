using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace MoonBot_Data
{
    public static class SubscriberD
    {
        public static void InsertFollowerFromExcel()
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"C:\Users\Nova\Documents\Subs.xlsx");
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            int r = xlRange.Rows.Count;
            int c = xlRange.Columns.Count;
                for (int i = 1; i <= r; i++)
                {
                    for (int j = 1; j <= c; j++)
                    {
                        //new line
                        if (j == 1)
                            Console.Write("\r\n");

                        //write the value to the console
                        if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                            Console.Write(xlRange.Cells[i, j].Value2.ToString() + "\t");

                        //add useful things here!   
                    }
                }
        }
    }
}
