using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonbot_Objects.User
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

    public class UserO
    {
        public int _total { get; set; }
        public List<User> users { get; set; }
    }

}
