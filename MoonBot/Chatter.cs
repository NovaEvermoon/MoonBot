using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoonBot
{
    public class Links
    {
    }

    public class Chatters
    {
        public IList<object> vips { get; set; }
        public IList<string> moderators { get; set; }
        public IList<object> staff { get; set; }
        public IList<object> admins { get; set; }
        public IList<object> global_mods { get; set; }
        public IList<string> viewers { get; set; }
    }

    public class Examplet
    {
        public Links _links { get; set; }
        public int chatter_count { get; set; }
        public Chatters chatters { get; set; }
    }
}
