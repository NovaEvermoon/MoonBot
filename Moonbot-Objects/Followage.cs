using Moonbot_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonbot_Objects
{
    public class Followage
    {
            public DateTime created_at { get; set; }
            public ChannelO channel { get; set; }
            public bool notifications { get; set; }
    }
}
