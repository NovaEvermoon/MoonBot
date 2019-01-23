using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoonBot
{
    public class Team
    {
        public int _id { get; set; }
        public string name { get; set; }
        public string info { get; set; }
        public string display_name { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string logo { get; set; }
        public string banner { get; set; }
        public object background { get; set; }
        public IList<Users> users { get; set; }

    }
}
