using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Boggle;
using Triepocalypse;

namespace BoggleGame
{
    public sealed class PeterTheGreat : IBogglePlayer
    {
        private const string TeamName = "Peter the Great";
        public string Name { get; }
        public Guid Id { get; }

        private int RandomSeed { get; set; }

        private Trie<int> Trie { get; }

        public PeterTheGreat()
        {
            Name = TeamName;
            Id = Guid.NewGuid();

            // Build trie from dictionary
            Trie = CreateTrie();
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

        public void Solve(BoggleBoard boggleBoard)
        {
            Console.WriteLine("Board:\n{0}", boggleBoard);
            
            foreach (string word in EnumerateWords(boggleBoard))
            {
                // QUESITON: What happens if a word is duplicated on the board?
                int score = BoggleServer.Instance.Score(Id, boggleBoard.Id, word);
                if (score >= 1)
                {
                    Console.WriteLine("{0,4:+#;-#;0}: {1}", score, word);
                }
            }

            Console.WriteLine();
        }

        private IEnumerable<string> EnumerateWords(BoggleBoard boggleBoard)
        {
            // Create structure to manage adjacent letters
            Board board = CreateBoard(boggleBoard);

            // Find all the words
            WordBuilder builder = new WordBuilder();
            foreach (string word in EnumerateWords(board.Letters, builder))
            {
                yield return word;
            }
        }

        private IEnumerable<string> EnumerateWords(IEnumerable<Letter> letters, WordBuilder builder)
        {
            foreach (Letter letter in letters)
            {
                // Only continue with this letter if the prefix exists
                if (Trie.Matcher.HasNext(letter.C))
                {
                    // Add the current letter to the prefix
                    Trie.Matcher.Next(letter.C);

                    // If the current prefix is a word, capture it
                    if (Trie.Matcher.IsMatch())
                    {
                        string word = Trie.Matcher.PrefixMatched;
                        yield return word;
                    }

                    // Create a new word builder to descend with
                    WordBuilder nextBuilder = new WordBuilder(builder);
                    nextBuilder.AddLetter(letter);

                    // Get adjacent letters that haven't already been used in this word
                    IEnumerable<Letter> nextLetters = letter.AdjacentLetters.Where(x => !nextBuilder.UsedLetters.Contains(x));

                    // Descend into the adjacent letters
                    foreach (string word in EnumerateWords(nextLetters, nextBuilder))
                    {
                        yield return word;
                    }

                    // Remove the current letter from the prefix
                    Trie.Matcher.Back();
                }
            }
        }

        private Board CreateBoard(BoggleBoard boggleBoard)
        {
            Board board = new Board();

            // Flatten the board matrix into a single-dimension list
            List<Letter> letters = new List<Letter>();

            for (int y = 0; y < boggleBoard.BoardSize; y++)
            {
                for (int x = 0; x < boggleBoard.BoardSize; x++)
                {
                    letters.Add(new Letter { X = x, Y = y, C = boggleBoard.Letters[x, y] });
                }
            }

            // Loop over the letters and identify the adjacent letters
            for (int i = 0; i < letters.Count; i++)
            {
                Letter letter = letters[i];
                letter.AdjacentLetters = GetAdjacentIndexes(i, boggleBoard.BoardSize).Select(x => letters[x]).ToList();
            }

            board.Letters = letters;
            return board;
        }

        private List<int> GetAdjacentIndexes(int index, int boardDimension)
        {
            List<int> adjacentIndexes = new List<int>();

            // Calculate indexes of positions adjacent to current letter (X)
            //    1 2 3
            //    4 X 6 
            //    7 8 9
            int index1 = index - (boardDimension + 1);
            int index2 = index - boardDimension;
            int index3 = index - (boardDimension - 1);
            int index4 = index - 1;
            int index6 = index + 1;
            int index7 = index + (boardDimension - 1);
            int index8 = index + boardDimension;
            int index9 = index + (boardDimension + 1);

            // Check if current letter is against any edges to exclude adjacents
            bool onTopEdge = (index < boardDimension);
            bool onBottomEdge = (index >= boardDimension * (boardDimension - 1));
            bool onLeftEdge = (index % boardDimension == 0);
            bool onRightEdge = (index % boardDimension == boardDimension - 1);

            if (!onTopEdge)
            {
                adjacentIndexes.Add(index2);

                if (!onLeftEdge)
                {
                    adjacentIndexes.Add(index1);
                }

                if (!onRightEdge)
                {
                    adjacentIndexes.Add(index3);
                }
            }

            if (!onBottomEdge)
            {
                adjacentIndexes.Add(index8);

                if (!onLeftEdge)
                {
                    adjacentIndexes.Add(index7);
                }

                if (!onRightEdge)
                {
                    adjacentIndexes.Add(index9);
                }
            }

            if (!onLeftEdge)
            {
                adjacentIndexes.Add(index4);
            }

            if (!onRightEdge)
            {
                adjacentIndexes.Add(index6);
            }

            return adjacentIndexes;
        }

        private Trie<int> CreateTrie()
        {
            int count = 0;

            // https://sourceforge.net/p/triepocalypse/wiki/Manual/
            Trie<int> trie = new Trie<int>();

            foreach (string word in BoggleRules.Instance.GetWordSet())
            {
                if (word.Length >= BoggleRules.WordLengthMin)
                {
                    // The value is not used, so storing the current word count
                    trie.Add(word, ++count);
                }
            }

            return trie;
        }
    }
}