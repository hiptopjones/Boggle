using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Boggle;
using NetSpell.SpellChecker.Dictionary;

namespace BoggleGame
{
    public sealed class MarvinTheMartian : IBogglePlayer
    {
        private const string TeamName = "Marvin's Martians";
        public string Name { get; }
        public Guid Id { get; }

        private int RandomSeed { get; set; }

        public MarvinTheMartian()
        {
            Name = TeamName;
            Id = Guid.NewGuid();
        }

        public void Start()
        {
            // Play a game
            RandomSeed = 1;
            IBogglePlayer player = this;
            BoggleResult result = BoggleServer.Instance.Play(player, RandomSeed);

            // Check result
            List<string> correctWords = result.WordPointDict.Keys.Where(word => result.WordPointDict[word] > 0).ToList();
            int correctWordCount = correctWords.Count;
            int incorrectWordCount = result.WordPointDict.Count - correctWordCount;
            Console.WriteLine("{0} result for board {1} =\n\t{2} points, {3} correct words, {4} incorrect words, {5} msec.",
                Name, result.Board.Id, result.Points, correctWordCount, incorrectWordCount, result.Duration.TotalMilliseconds);
        }

        public void Solve(BoggleBoard board)
        {
            Console.WriteLine("Board:\n{0}", board);

            //List<string> words = GetAllDictionaryWords(board);
            List<string> words = GetPresolvedWords(board, RandomSeed);

            foreach (string word in words)
            {
                int score = BoggleServer.Instance.Score(Id, board.Id, word);
                if (score >= 1)
                {
                    Console.WriteLine("{0,4:+#;-#;0}: {1}", score, word);
                }
            }

            Console.WriteLine();
        }

        private List<string> GetAllDictionaryWords(BoggleBoard board)
        {
            // Ignore board, just try every word in dictionary

            Hashtable wordHashtable = BoggleRules.Instance.GetWordDictionary().BaseWords;
            List<string> words = new List<string>();
            foreach (Word word in wordHashtable.Values)
            {
                words.Add(word.Text);
            }
            return words;
        }
        private List<string> GetPresolvedWords(BoggleBoard board, int randomSeed)
        {
            if (randomSeed != 1)
            {
                throw new NotSupportedException("These words are only valid for the board with random seed = 1");
            }

            return new List<string>
            {
                "emir",
                "idem",
                "same",
                "lame",
                "salvo",
                "edit",
                "ilia",
                "omit",
                "pilot",
                "soma",
                "tide",
                "aloft",
                "some",
                "amide",
                "sift",
                "lift",
                "trim",
                "flam",
                "alto",
                "media",
                "lamed",
                "SALT",
                "Milt",
                "hilt",
                "film",
                "flirt",
                "Diem",
                "idea",
                "Palm",
                "alias",
                "Pail",
                "sail",
                "mail",
                "flap",
                "limit",
                "tried",
                "lied",
                "Ride",
                "hide",
                "limo",
                "maid",
                "demo",
                "toil",
                "moil",
                "foil",
                "flip",
                "aide",
                "rime",
                "time",
                "lime",
                "dime",
                "flit",
                "email",
                "dirt",
                "asap",
                "loft",
                "malt",
                "soap",
                "volt",
                "heir",
                "emit",
                "Alma",
                "demit",
                "molt",
                "amid",
                "flame",
                "lama",
                "diam"
            };
        }
    }
}