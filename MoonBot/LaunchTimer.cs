using MoonBot_Data;
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
        public List<CustomTimer> timers;
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

            timers = new List<CustomTimer>();
        }

        public LaunchTimer(IrcClient irc)
        {
            this.irc = irc;
            timers = new List<CustomTimer>();
        }
        public void createTimer(CommandO command)
        {

                CustomTimer timer = new CustomTimer(command.timer, command.message);
                timer.Start();
                timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
                timers.Add(timer);
        }

        public void createTimer()
        {
            CustomTimer timer = new CustomTimer(miliseconds);
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
                    if (sender is CustomTimer)
                        irc.WriteChatMessage(((CustomTimer)sender).commandMessage);
        }
    }

    public class CustomTimer : Timer
    {
        public string commandMessage;
        public UserO user;

        public CustomTimer(double miliseconds) : base(miliseconds)
        {
            
        }

        public CustomTimer(double miliseconds, string commandMessage) : base(miliseconds)
        {
            this.commandMessage = commandMessage;
        }

        public CustomTimer(double miliseconds, UserO user) : base(miliseconds)
        {
            this.user = user;
        }
    }   
    
}
