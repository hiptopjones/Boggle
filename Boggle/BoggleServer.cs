using System;
using System.Collections.Generic;
using System.Linq;

namespace Boggle
{
    public sealed class BoggleServer : IBoggleServer
    {
        private IBoggleRules Rules { get; }
        private HashSet<string> WordSet { get; }
        private List<BoggleTracker> Trackers { get; }

        private static readonly Lazy<BoggleServer> InstanceHolder = new Lazy<BoggleServer>(() => new BoggleServer());

        public static BoggleServer Instance => InstanceHolder.Value;

        private BoggleServer() : this(BoggleRules.Instance)
        {
        }

        internal BoggleServer(IBoggleRules rules)
        {
            Rules = rules;
            WordSet = Rules.GetWordSet();
            Trackers = new List<BoggleTracker>();
        }

        public BoggleResult Play(IBogglePlayer player, int randomSeed = 0, int boardSize = BoggleRules.BoardSizeDefault)
        {
            BoggleBoard board = Rules.GenerateBoard(randomSeed, boardSize);
            BoggleTracker tracker = CreateTracker(player, board);
            tracker.Start();

            try
            {
                player.Solve(board);
                tracker.Stop();
            }
            catch (Exception)
            {
                tracker.Stop();
                tracker.Update(string.Empty, BoggleRules.CatastrophicScore);
                throw;
            }

            BoggleResult result = CreateResult(tracker);
            DeleteTracker(player.Id, board.Id);
            return result;
        }

        public int TotalScore(Guid playerId, Guid boardId)
        {
            BoggleTracker tracker = FindTracker(playerId, boardId);
            return tracker.Points;
        }

        public int Score(Guid playerId, Guid boardId, string word)
        {
            int points;
            word = word.ToLower();
            BoggleTracker tracker = FindTracker(playerId, boardId);
            if (tracker.WordPointDict.TryGetValue(word, out points))
            {
                return 0;
            }

            if (IsWordInDictionary(word))
            {
                if (tracker.Board.FindWord(word) != null)
                {
                    points = Rules.Score(word.Length);
                }
                else
                {
                    points = BoggleRules.IncorrectWordScore;
                }
            }
            else
            {
                points = BoggleRules.IncorrectWordScore;
            }

            tracker.Update(word, points);

            return points;
        }

        private BoggleTracker CreateTracker(IBogglePlayer player, BoggleBoard board)
        {
            BoggleTracker tracker = new BoggleTracker(player.Id, board);
            Trackers.Add(tracker);
            return tracker;
        }

        private BoggleTracker FindTracker(Guid playerId, Guid boardId)
        {
            BoggleTracker tracker = Trackers.FirstOrDefault(t => t.PlayerId == playerId && t.Board.Id == boardId);
            return tracker;
        }

        private void DeleteTracker(Guid playerId, Guid boardId)
        {
            BoggleTracker tracker = Trackers.FirstOrDefault(t => t.PlayerId == playerId && t.Board.Id == boardId);
            Trackers.Remove(tracker);
        }

        private bool IsWordInDictionary(string word)
        {
            bool isValidWord = WordSet.Contains(word.ToLower());
            return isValidWord;
        }

        private BoggleResult CreateResult(BoggleTracker tracker)
        {
            BoggleResult result = new BoggleResult(tracker.Board, tracker.WordPointDict,
                tracker.Points, tracker.Stopwatch.Elapsed);
            return result;
        }
    }
}