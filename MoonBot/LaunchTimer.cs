using Moonbot_Objects.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MoonBot
{
    public class LaunchTimer
    {
        public List<customTimer> timers;
        private IrcClient irc;

        public LaunchTimer(IrcClient irc)
        {
            this.irc = irc;
            timers = new List<customTimer>();
        }
        public void createTimer(CommandO command)
        {
            customTimer timer = new customTimer(command.timer, command.message);
            timer.Start();
            timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            timers.Add(timer);
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(sender != null)
                if(sender is customTimer)
                    irc.WriteChatMessage(((customTimer)sender).commandMessage);
        }
    }

    public class customTimer : Timer
    {
        public string commandMessage;

        public customTimer(double miliseconds, string commandMessage) : base(miliseconds)
        {
            this.commandMessage = commandMessage;
        }
    }
}
