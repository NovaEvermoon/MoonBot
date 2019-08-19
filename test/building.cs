using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
   public  class building
    {
        int nbWalls { get; set; }
        int nbBeams { get; set; }


        public building()
        {
            nbWalls = 4;
            nbBeams = 10;
        }
    }
}
