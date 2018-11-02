using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoonBot
{
    public static class ChatBot
    {
        public static string botName = "themoonchatbot";
        public static string password = "oauth:mmidl67j5h7ygb4gfjbccxwrb502bo";
        public static string clientID = "v31jt30fnjpg8qgu0ucfbd3ly0q5tx";
        public static string twitchId = "167461349";
        public static string broadcasterName = "novaevermoon";

        public static string GetMessage(string fullMessage)
        {
            string message;
            int intIndexParseSign = fullMessage.IndexOf(" :");
            message = fullMessage.Substring(intIndexParseSign + 2);

            return message;
        }

        public static string GetUsername(string fullMessage)
        {
            string username;
            int intIndexParseSign = fullMessage.IndexOf('!');
            username = fullMessage.Substring(1, intIndexParseSign - 1);

            return username;
        }

        public static void WriteChatLogLine(string username, string message)
        {

        }
    }
}
