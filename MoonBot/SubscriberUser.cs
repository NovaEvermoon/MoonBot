using System;
using System.Collections.Generic;

public class SubscriberUser
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
    public SubscriberUser user { get; set; }
    public object sender { get; set; }
}

public class Subs
{
    public int _total { get; set; }
    public IList<Subscription> subscriptions { get; set; }
}
