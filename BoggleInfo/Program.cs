using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Boggle;

namespace BoggleInfo
{
    public class Program
    {
        static void Main(string[] args)
        {
            const int boardSize = 5;
            BoggleBoard board = new BoggleBoard(boardSize: boardSize);

            Console.WriteLine("Board:");
            Console.WriteLine(board);

            Console.WriteLine("Press any key to exit. ");
            Console.ReadKey();
        }

        private static void DumpNeighborsList(List<Point>[,] neighborsList)
        {
            int rows = neighborsList.GetLength(0);
            int columns = neighborsList.GetLength(1);

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    Console.Write("Neighbors of [{0}][{1}] = ", row, column);
                    List<Point> neighbors = neighborsList[row, column];
                    DumpNeighbors(neighbors);
                }
            }
        }

        private static void DumpNeighbors(List<Point> neighbors)
        {
            string[] neighborPoints = neighbors.Select(n => $"[{n.X}, {n.Y}]").ToArray();
            Console.WriteLine(string.Join(", ", neighborPoints));
        }
    }
}
