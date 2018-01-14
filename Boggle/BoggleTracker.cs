using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Boggle
{
    internal class BoggleTracker
    {
        public BoggleTracker(Guid playerId, BoggleBoard board)
        {
            PlayerId = playerId;
            Board = board;
            WordPointDict = new Dictionary<string, int>();
            Points = 0;
            Stopwatch = new Stopwatch();
        }

        public Guid PlayerId { get; }
        public BoggleBoard Board { get; }
        public Dictionary<string, int> WordPointDict { get; }
        public int Points { get; private set; }
        public Stopwatch Stopwatch { get; }

        public void Start()
        {
            Stopwatch.Start();
        }

        public void Stop()
        {
            Stopwatch.Stop();
            var points = WordPointDict.Values.ToList();
            Points = points.Sum();
        }

        public void Update(string word, int points)
        {
            if (!string.IsNullOrEmpty(word) && (points != 0))
                WordPointDict[word] = points;
        }
    }
}