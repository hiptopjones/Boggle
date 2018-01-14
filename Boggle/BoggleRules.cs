using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Boggle
{
    public sealed class BoggleRules : IBoggleRules
    {
        public const int BoardSizeMin = 4;
        public const int BoardSizeMax = 5;
        public const int BoardSizeDefault = 5;

        public const int WordLengthMin = 4;
        public const int WordLengthMax = 8;
        public const int IncorrectWordScore = -5;
        public const int CatastrophicScore = -50;

        public const int DiceFaceCount = 6;

        private static readonly List<int> ScoringTable = new List<int>
        {
            IncorrectWordScore,
            IncorrectWordScore,
            IncorrectWordScore,
            IncorrectWordScore,
            1, // 4
            2, // 5
            3, // 6
            5, // 7
            11 // 8
        };

        private static readonly List<char[]> DiceSet = new List<char[]>
        {
            "SEPTIC".ToLower().ToCharArray(),
            "SUSSEN".ToLower().ToCharArray(),
            "PICLET".ToLower().ToCharArray(),
            "AFARAS".ToLower().ToCharArray(),
            "EEMEEA".ToLower().ToCharArray(),
            "FRISPY".ToLower().ToCharArray(),
            "HORNDL".ToLower().ToCharArray(),
            "WORVGR".ToLower().ToCharArray(),
            "ZXJKBQ".ToLower().ToCharArray(),
            "EGAMEU".ToLower().ToCharArray(),
            "TIETII".ToLower().ToCharArray(),
            "EEAEEA".ToLower().ToCharArray(),
            "TONHDH".ToLower().ToCharArray(),
            "TOOTOU".ToLower().ToCharArray(),
            "FAIRYS".ToLower().ToCharArray(),
            "ANNEND".ToLower().ToCharArray(),
            "FARISA".ToLower().ToCharArray(),
            "PRRHIY".ToLower().ToCharArray(),
            "HOHRLD".ToLower().ToCharArray(),
            "WOUTON".ToLower().ToCharArray(),
            "MOTETT".ToLower().ToCharArray(),
            "DLORND".ToLower().ToCharArray(),
            "CCSNWT".ToLower().ToCharArray(),
            "LICTIE".ToLower().ToCharArray(),
            "ANNGEM".ToLower().ToCharArray()
        };

        private static readonly Lazy<BoggleRules> InstanceHolder = new Lazy<BoggleRules>(() => new BoggleRules());
        public static BoggleRules Instance => InstanceHolder.Value;

        private BoggleRules()
        {
        }

        public BoggleBoard GenerateBoard(int randomSeed = 0, int boardSize = BoggleRules.BoardSizeDefault)
        {
            return new BoggleBoard(randomSeed: randomSeed, boardSize: boardSize);
        }

        public List<char[]> GetDiceSet(int boardSize = BoardSizeDefault)
        {
            CheckBoardSize(boardSize);
            return new List<char[]>(DiceSet.GetRange(0, boardSize*boardSize));
        }

        public void CheckBoardSize(int boardSize)
        {
            if ((boardSize < BoardSizeMin) || (boardSize > BoardSizeMax))
                throw new NotSupportedException($"Board size must be between {BoardSizeMin} and {BoardSizeMax}.");
        }

        public HashSet<string> GetWordSet()
        {

            HashSet<string> wordSet = new HashSet<string>();
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            foreach (char c in alphabet.ToCharArray())
            {
                // http://dreamsteep.com/projects/the-english-open-word-list.html
                wordSet.UnionWith(GetWordsFromFile(string.Format("{0} Words.txt", c)));
            }

            return wordSet;
        }

        private IEnumerable<string> GetWordsFromFile(string filename)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = string.Format("Boggle.Words.{0}", filename);

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                while(!reader.EndOfStream)
                {
                    string word = reader.ReadLine();
                    if (!string.IsNullOrWhiteSpace(word))
                    {
                        yield return word;
                    }
                }
            }
        }

        public int Score(int wordLength)
        {
            wordLength = Math.Max(wordLength, 0);
            wordLength = Math.Min(wordLength, WordLengthMax);
            return ScoringTable[wordLength];
        }
    }
}