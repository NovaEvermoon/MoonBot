using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TwitchLib.Api;
using TwitchLib.Api.Models.Helix.Users.GetUsersFollows;
using TwitchLib.Api.Models.v5.Channels;
using TwitchLib.Api.Models.v5.Subscriptions;

namespace MoonBot
{
    public static class ChatBot
    {

        public static string botName = "themoonchatbot";
        public static string headerl1 = @"• ▌ ▄ ·.              ▐ ▄ ▄▄▄▄·       ▄▄▄▄▄";
        public static string headerl2 = @"·██ ▐███▪▪     ▪     •█▌▐█▐█ ▀█▪▪     •██ ";
        public static string headerl3 = @"▐█ ▌▐▌▐█· ▄█▀▄  ▄█▀▄ ▐█▐▐▌▐█▀▀█▄ ▄█▀▄  ▐█.▪";
        public static string headerl4 = @"██ ██▌▐█▌▐█▌.▐▌▐█▌.▐▌██▐█▌██▄▪▐█▐█▌.▐▌ ▐█▌·";
        public static string headerl5 = @"▀▀  █▪▀▀▀ ▀█▄▀▪ ▀█▄▀▪▀▀ █▪·▀▀▀▀  ▀█▄▀▪ ▀▀▀ ";

        public static string headerSeparator = @"｡･:*:･ﾟ ★,｡･:*:･ﾟ☆ﾟ･:*:･｡,★ ﾟ･:*:･｡･:*:･ﾟ ★,｡･:*:･ﾟ☆ﾟ･:*:･｡,★ ﾟ･:*:･｡";
        public static string GetMessage(string fullMessage)
        {
            string message;
            message = fullMessage.Substring(fullMessage.IndexOf('#'));
            message = message.Substring(message.IndexOf(':') + 1 );

            return message;
        }

        

        public static void WriteChatLogLine(string username, string message)
        {

        }


        public static bool checkLink(string message)
        {
            bool link = false;
            Regex regex = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            foreach (Match m in regex.Matches(message))
            {
                if (m != null)
                {
                    link = true;
                }
            }

            return link;
        }



        public static void init()
        {
            string version = ConfigurationManager.AppSettings["version"];
            Console.OutputEncoding = Encoding.UTF8;
            Console.SetCursorPosition((Console.WindowWidth - (headerl1.Length)) / 2, Console.CursorTop+1);
            Console.Write(headerl1);
            Console.SetCursorPosition((Console.WindowWidth - (headerl2.Length)) / 2, Console.CursorTop+1);
            Console.Write(headerl2);
            Console.SetCursorPosition((Console.WindowWidth - (headerl3.Length)) / 2, Console.CursorTop+1);
            Console.Write(headerl3);
            Console.SetCursorPosition((Console.WindowWidth - (headerl4.Length)) / 2, Console.CursorTop+1);
            Console.Write(headerl4);
            Console.SetCursorPosition((Console.WindowWidth - (headerl5.Length)) / 2, Console.CursorTop+1);
            Console.Write(headerl5);
            Console.WriteLine();
            Console.SetCursorPosition((Console.WindowWidth - (headerSeparator.Length)) / 2, Console.CursorTop + 1);
            Console.Write(headerSeparator);
            Console.SetCursorPosition(0, Console.CursorTop + 1);
            Console.WriteLine();

            int test = Console.CursorLeft;
            int test2 = Console.CursorTop;
            Console.Title = "Moonbot v."+version;
        }
    }
}
