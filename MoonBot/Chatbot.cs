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
        public static string password = "oauth:mmidl67j5h7ygb4gfjbccxwrb502bo";
        public static string clientID = "v31jt30fnjpg8qgu0ucfbd3ly0q5tx";
        public static string channelId = "167461349";
        public static string broadcasterName = "novaevermoon";
        

        public static string GetMessage(string fullMessage)
        {
            string message;
            message = fullMessage.Substring(fullMessage.IndexOf('#'));
            message = message.Substring(message.IndexOf(':') + 1 );

            return message;
        }

        public static string GetUsername(string fullMessage)
        {
            string username;
            int intIndexParseSign = fullMessage.IndexOf(':');
            if(fullMessage.Contains('!'))
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
