using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MoonBot
{
    class TmiApi
    {
        public Example getMods()
        {
            Example test = new Example();
            string url = "https://tmi.twitch.tv/group/user/" + ChatBot.broadcasterName + "/chatters";
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            if (webRequest != null)
            {
                try
                {
                    using (Stream s = webRequest.GetResponse().GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(s))
                        {
                            var jsonResponse = sr.ReadToEnd();
                            test = JsonConvert.DeserializeObject<Example>(jsonResponse);
                        }
                    }
                }
                catch(Exception ex)
                {
                    
                }
            }

            return test;
        }
    }
}
