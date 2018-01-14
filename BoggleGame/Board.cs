using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleGame
{
    class Board
    {
        public List<Letter> Letters { get; set; }

        public Board()
        {
            Letters = new List<Letter>();
        }
    }
}
