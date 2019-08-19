using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class House : building
    {
        public int nbWindows;
        public int nbDoors;

        public House()
        {
            nbWindows = 2;
            nbDoors = 1;
        }
    }
}
