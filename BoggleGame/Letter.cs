using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleGame
{
    public class Letter
    {
        public char C { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public List<Letter> AdjacentLetters { get; set; }
    }
}
