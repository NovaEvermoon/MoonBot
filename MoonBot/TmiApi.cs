
using Moonbot_Objects;
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
    public class TmiApi
    {
        public ViewerList getViewerList(ChannelO channel)
        {
            ViewerList test = new ViewerList();
            string url = "https://tmi.twitch.tv/group/user/" + channel.name + "/chatters";
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
                            test = JsonConvert.DeserializeObject<ViewerList>(jsonResponse);
                        }
                    }
                }
                catch (Exception ex)
                {
                    StringBuilder sb = new StringBuilder(DateTime.Now.ToString("dd-MM-yyyy") + " : " + ex.Message);
                    Console.WriteLine(sb);
                }
            }

            return test;
        }

        public List<string> getViewers(ViewerList viewerList)
        {
            List<string> viewers = new List<string>();

            foreach(string viewer in viewerList.chatters.viewers)
            {
                viewers.Add(viewer);
            }

            return viewers;
        }

        public List<string> getMods(ViewerList viewerList)
        {
            List<string> mods = new List<string>();

            foreach (string viewer in viewerList.chatters.viewers)
            {
                mods.Add(viewer);
            }

            return mods;
        }

    }
}
