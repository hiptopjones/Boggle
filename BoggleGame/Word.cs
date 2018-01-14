using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleGame
{
    public class WordBuilder
    {
        public HashSet<Letter> UsedLetters { get; set; }

        public WordBuilder()
        {
            UsedLetters = new HashSet<Letter>();
        }

        public WordBuilder(WordBuilder builder)
        {
            // Copy the items, but not a deep copy
            UsedLetters = new HashSet<Letter>(builder.UsedLetters);
        }

        public void AddLetter(Letter letter)
        {
            UsedLetters.Add(letter);
        }
    }
}
