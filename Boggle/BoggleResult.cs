using System;
using System.Collections.Generic;

namespace Boggle
{
    public sealed class BoggleResult
    {
        public BoggleBoard Board { get; }
        public Dictionary<string, int> WordPointDict { get; }
        public int Points { get; }
        public TimeSpan Duration { get; }

        public BoggleResult(BoggleBoard board, Dictionary<string, int> wordPointDict, int points, TimeSpan duration)
        {
            Board = board;
            WordPointDict = wordPointDict;
            Points = points;
            Duration = duration;
        }
    }
}