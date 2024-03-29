﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonbot_Objects
{
    public class CommandO
    {
        public int id { get; set; }
        public string keyword { get; set; }
        public string message { get; set; }
        public string userLevel { get; set; }
        public int cooldown { get; set; }
        public bool status { get; set; }
        public int timer { get; set; }
        public string description { get; set; }
        public DateTime startedTime { get; set; }
        public string type { get; set; }
        public string request { get; set; }
        public int parameters { get; set; }
        public Dictionary<string, dynamic> parameterList { get; set; }
        public string assembly { get; set; }
        public string condition { get; set; }

        public CommandO()
        {
            parameterList = new Dictionary<string, dynamic>();
        }
    }
}
