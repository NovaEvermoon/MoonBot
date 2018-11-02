using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MoonBot
{
    public class IrcClient
    {

        public string botName;
        private string channelName;

        private TcpClient tcpClient;
        private StreamReader reader;
        private StreamWriter writer;

        public IrcClient(string Ip, int Port, string BotName, string Password, string ChannelName)
        {
            try
            {
                this.botName = BotName;
                this.channelName = ChannelName;

                tcpClient = new TcpClient(Ip, Port);
                reader = new StreamReader(tcpClient.GetStream());
                writer = new StreamWriter(tcpClient.GetStream());

                // Try to join the room
                writer.WriteLine("PASS " + Password);
                writer.WriteLine("NICK " + BotName);
                writer.WriteLine("USER " + BotName + " 8 * :" + BotName);
                writer.WriteLine("JOIN #" + ChannelName);
                writer.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                Console.WriteLine(ex.Message);
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
                Console.WriteLine(ex.Message);
            }
        }

        public string ReadMessage()
        {
            try
            {
                string message = reader.ReadLine();
                return message;
            }
            catch (Exception ex)
            {
                return "Error receiving message: " + ex.Message;
            }
        }
    }
}
