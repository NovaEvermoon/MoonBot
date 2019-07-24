using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace MoonBot
{
    public class IrcClient
    {

        public string botName;
        private string channelName;

        private TcpClient tcpClient;
        private StreamReader ircReader;
        private StreamWriter writer;

        public IrcClient(string Ip, int Port, string BotName, string Password, string ChannelName)
        {
            try
            {
                this.botName = BotName;
                this.channelName = ChannelName;

                tcpClient = new TcpClient(Ip, Port);
                ircReader = new StreamReader(tcpClient.GetStream());
                writer = new StreamWriter(tcpClient.GetStream());

                writer.WriteLine("PASS " + Password);
                writer.WriteLine("NICK " + BotName);
                writer.WriteLine("USER " + BotName + " 8 * :" + BotName);
                writer.WriteLine("JOIN #" + ChannelName);
                writer.Flush();
            }
            catch (Exception ex)
            {

                StringBuilder sb = new StringBuilder(DateTime.Now.ToString("dd-MM-yyyy") + " : " + ex.Message);
                Console.WriteLine(sb);
            }
        }

        public void WriteConsoleMessage(string Message)
        {
            try
            {
                writer.WriteLine(Message);
                writer.Flush();
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder(DateTime.Now.ToString("dd-MM-yyyy") + " : " + ex.Message);
                Console.WriteLine(sb);
            }
        }

        public void WriteChatMessage(string message)
        {
            try
            {
                WriteConsoleMessage(":" + botName + "!" + botName + "@" + botName +
                ".tmi.twitch.tv PRIVMSG #" + channelName + " :" + message);
            }
            catch (Exception ex)
            {

                StringBuilder sb = new StringBuilder(DateTime.Now.ToString("dd-MM-yyyy") + " : " + ex.Message);
                Console.WriteLine(sb);
            }
        }

        public void getMod()
        {
            WriteChatMessage(":" + botName + "!" + botName + "@" + botName + ".tmi.twitch.tv GLOBALUSERSTATE #" + channelName);
        }

        public string ReadMessage()
        {
            string message;
            try
            {
                message = ircReader.ReadLine();

                return message;
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder(DateTime.Now.ToString("dd-MM-yyyy") + " : " + ex.Message);
                return sb.ToString();
            }

            return message;
        }

        public string Join()
        {
            StringBuilder returndata = new StringBuilder();
            writer.WriteLine("/join #" + channelName);

            while(!returndata.ToString().Contains("End of /NAMES list"))
            {
                returndata.Append(ReadMessage());
            }

            return returndata.ToString(); ;

        }
    }
}
