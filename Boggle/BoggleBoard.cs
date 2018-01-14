using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Boggle
{
    public sealed class BoggleBoard
    {
        public Guid Id { get; }
        public char[,] Letters { get; }
        public int BoardSize { get; }
        private Random Random { get; }

        private int RandomSeed { get; }
        private List<Point>[,] Neighbors { get; }

        public BoggleBoard(int randomSeed = 0, int boardSize = BoggleRules.BoardSizeDefault)
        {
            BoardSize = boardSize;
            RandomSeed = randomSeed;
            Id = Guid.NewGuid();
            Random = RandomSeed == 0 ? new Random() : new Random(randomSeed);
            Letters = GenerateLetters(BoardSize);
            Neighbors = CreateNeighbors(BoardSize);
        }

        public override string ToString()
        {
            string board = string.Empty;
            for (int row = 0; row < BoardSize; row++)
            {
                for (int column = 0; column < BoardSize; column++)
                {
                    board += Letters[row, column];
                }
                board += '\n';
            }
            return board;
        }

        public static List<Point>[,] CreateNeighbors(int boardSize)
        {
            List<Point>[,] neighborsList = new List<Point>[boardSize, boardSize];

            for (int row = 0; row < boardSize; row++)
            {
                for (int column = 0; column < boardSize; column++)
                {
                    var neighbors = CreateNeighbors(boardSize, row, column);
                    neighborsList[row, column] = neighbors;
                }
            }

            return neighborsList;
        }

        private static List<Point> CreateNeighbors(int boardSize, int row, int column)
        {
            List<Point> neighbors = new List<Point>();

            int rowMin = Math.Max(0, row - 1);
            int rowMax = Math.Min(boardSize - 1, row + 1);

            for (int rowNext = rowMin; rowNext <= rowMax; rowNext++)
            {
                int columnMin = Math.Max(0, column - 1);
                int columnMax = Math.Min(boardSize - 1, column + 1);

                for (int columnNext = columnMin; columnNext <= columnMax; columnNext++)
                {
                    if ((rowNext == row) && (columnNext == column))
                    {
                        continue;
                    }
                    neighbors.Add(new Point(rowNext, columnNext));
//                  Console.WriteLine("[{0},{1}] => [{2},{3}]", row, column, rowNext, columnNext);
                }
            }
            return neighbors;
        }

        internal List<Point> FindWord(string word)
        {
            List<List<Point>> pathList = FindFirstLetter(word[0]);
            if (pathList.Count == 0)
            {
                return null;
            }

            for (int index = 1; index < word.Length; index++)
            {
                char letter = word[index];
                pathList = FindAdjacentLetter(letter, pathList, index-1);
                if (pathList.Count == 0)
                {
                    return null;
                }
            }

            return pathList.Count == 0 ? null : pathList[0];
        }

        private List<List<Point>> FindAdjacentLetter(char letter, List<List<Point>> pathList, int index)
        {
            List<List<Point>> newPathList = new List<List<Point>>();
            foreach (List<Point> path in pathList)
            {
                Point lastPoint = path[index];
                List<Point> neighbors = Neighbors[lastPoint.X, lastPoint.Y];
                List<Point> matches = neighbors.Where(p => (letter == Letters[p.X, p.Y]) && !path.Contains(p)).ToList();
                foreach (Point match in matches)
                {
                    List<Point> newPath = new List<Point>(path) {match};
                    newPathList.Add(newPath);
                }
            }
            return newPathList;
        }

        private List<List<Point>> FindFirstLetter(char letter)
        {
            List<List<Point>> pathList = new List<List<Point>>();
            int rows = BoardSize;
            int columns = BoardSize;
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    if (letter == Letters[row, column])
                    {
                        pathList.Add(new List<Point> {new Point(row, column)});
                    }
                }
            }
            return pathList;
        }

        private char[,] GenerateLetters(int boardSize)
        {
            BoggleRules.Instance.CheckBoardSize(boardSize);

            var rowCount = boardSize;
            var columnCount = boardSize;
            var letters = new char[rowCount, columnCount];

            var diceSet = BoggleRules.Instance.GetDiceSet(boardSize);

            for (var row = 0; row < rowCount; row++)
                for (var column = 0; column < columnCount; column++)
                {
                    var dice = GetRandomDice(ref diceSet);
                    var letter = GetRandomLetter(dice);
                    letters[row, column] = letter;
                }

            return letters;
        }

        private char[] GetRandomDice(ref List<char[]> diceSet)
        {
            var diceIndexMax = diceSet.Count;
            var diceIndex = Random.Next(0, diceIndexMax);
            var dice = diceSet[diceIndex];
            diceSet.RemoveAt(diceIndex);
            return dice;
        }

        private char GetRandomLetter(char[] dice)
        {
            var faceIndex = Random.Next(0, dice.Length);
            var letter = dice[faceIndex];
            return letter;
        }
    }
}