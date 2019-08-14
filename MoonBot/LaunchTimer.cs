using Moonbot_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MoonBot
{
    public class LaunchTimer
    {
        public List<customTimer> timers;
        private IrcClient irc;
        private string functionName;
        private string assemblyName;
        private string[] parameters;
        private int miliseconds;
        public int result;

        public LaunchTimer(string functionName, string assemblyName, string[] parameters, int miliseconds)
        {
            this.assemblyName = assemblyName;
            this.functionName = functionName;
            this.parameters = parameters;
            this.miliseconds = miliseconds;

            timers = new List<customTimer>();
        }

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

        public void createTimer()
        {
            customTimer timer = new customTimer(miliseconds);
            timer.Start();
            timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed_With_Result);
            timers.Add(timer);
        }

        public void _timer_Elapsed_With_Result(object sender, ElapsedEventArgs e)
        {
            if (sender != null)
                if (sender is Timer)
                {
                    try
                    {
                        MethodInfo mInfo;
                        Type type = Assembly.Load("MoonBot_Data").GetType(assemblyName, false, true);
                        mInfo = type.GetMethod(functionName);

                        object answer = mInfo.Invoke(null, parameters);
                        this.result = Convert.ToInt32(answer);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
        }
        

        
        public void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
                if (sender != null)
                    if (sender is customTimer)
                        irc.WriteChatMessage(((customTimer)sender).commandMessage);
        }
    }

    public class customTimer : Timer
    {
        public string commandMessage;

        public customTimer(double miliseconds) : base(miliseconds)
        {
            
        }

        public customTimer(double miliseconds, string commandMessage) : base(miliseconds)
        {
            this.commandMessage = commandMessage;
        }
    }   
    
}
