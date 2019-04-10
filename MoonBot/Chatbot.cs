using System;
using System.Collections.Generic;
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

        public static void getChannelID()
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
    }
}
