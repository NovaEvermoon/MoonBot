using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonbot_Objects
{
    public class UserSub
    {
        public string display_name { get; set; }
        public string type { get; set; }
        public string bio { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string name { get; set; }
        public string _id { get; set; }
        public string logo { get; set; }
    }

    public class Subscription
    {
        public DateTime created_at { get; set; }
        public string _id { get; set; }
        public string sub_plan { get; set; }
        public string sub_plan_name { get; set; }
        public bool is_gift { get; set; }
        public UserSub user { get; set; }
        public object sender { get; set; }
    }

    public class SubscriberO
    {
        public int _total { get; set; }
        public List<Subscription> subscriptions { get; set; }
    }

    public class SubscriptionUser
    {
        public string broadcaster_id { get; set; }
        public string broadcaster_name { get; set; }
        public bool is_gift { get; set; }
        public string plan_name { get; set; }
        public string tier { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
    }

    public class Subscriptions
    {
        public List<SubscriptionUser> data { get; set; }
    }
}
