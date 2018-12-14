using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoonBot
{
    public class User
    {
        public IList<Data> data { get; set; }
    }
}


public class TwitchUser
{
    public string _id { get; set; }
    public string bio { get; set; }
    public DateTime created_at { get; set; }
    public string display_name { get; set; }
    public string logo { get; set; }
    public string name { get; set; }
    public string type { get; set; }
    public DateTime updated_at { get; set; }
}

public class Subscription
{
    public string _id { get; set; }
    public DateTime created_at { get; set; }
    public string sub_plan { get; set; }
    public string sub_plan_name { get; set; }
    public TwitchUser user { get; set; }
}
