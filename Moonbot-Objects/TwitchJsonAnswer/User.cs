using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonbot_Objects.TwitchJsonAnswer
{
    public class Datum
    {
        public string id { get; set; }
        public string login { get; set; }
        public string display_name { get; set; }
        public string type { get; set; }
        public string broadcaster_type { get; set; }
        public string description { get; set; }
        public string profile_image_url { get; set; }
        public string offline_image_url { get; set; }
        public int view_count { get; set; }
    }

    public class User
    {
        List<Datum> data;
    }
}
