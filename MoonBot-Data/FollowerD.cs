using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Moonbot_Objects.Channel;
using MoonBot_Data.Channel;
using Moonbot_Objects.User;

namespace MoonBot_Data
{
    public static class FollowerD
    {
        public static void insertFollowers(ChannelO channel)
        {
            int offset = 0;
            List<User> followers = new List<User>();
            FollowerO followersO =  ChannelD.getChannelFollowers(offset, channel);
            int total = followersO._total;

            

            while(offset < total)
            {
                foreach (Follow follow in followersO.follows)
                {
                    followers.Add(follow.user);
                    offset += 1;
                    
                }
                followersO = ChannelD.getChannelFollowers(offset, channel);

            }

        }
    }
}
