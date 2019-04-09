using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonbot_Objects
{
    public class User
    {
        public string display_name { get; set; }
        public string _id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public object bio { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string logo { get; set; }
    }

    public class Follow
    {
        public DateTime created_at { get; set; }
        public bool notifications { get; set; }
        public User user { get; set; }
    }

    public class ExampleA
    {
        public int _total { get; set; }
        public string _cursor { get; set; }
        public IList<Follow> follows { get; set; }
    }

}
