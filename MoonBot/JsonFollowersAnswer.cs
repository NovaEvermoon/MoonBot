using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoonBot
{
    public class JsonFollowersAnswer
    {
        public int total { get; set; }
        public IList<DataJsonFollowers> data { get; set; }
        public Pagination pagination { get; set; }
    }
    
    public class Pagination
    {
        public string cursor { get; set; }   
    }
    public class DataJsonFollowers
    {
        public string from_name { get; set; }
        public string to_id { get; set; }
        public string to_name { get; set; }
        public DateTime followed_at { get; set; }
        public string from_id { get; set; }
    }

}
