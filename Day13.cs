using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public static class Day13
    {
        /// <summary>
        /// represents the 0th and 1st dimension of our 2d matrix thats folding
        /// </summary>
        enum Direction : int {
            y=0,
            x=1
        }
        static int y(this ValueTuple<int, int> t)
        {
            return t.Item1;
        }
        static int x(this ValueTuple<int, int> t)
        {
            return t.Item2;
        }

        private static int[,] FoldY(int[,] arr, int line)
        {
            var res = new int[line, arr.GetLength(1)];
            for (int i = 0; i < res.GetLength(0); i++)
            {
                for (int j = 0; j < res.GetLength(1); j++)
                {
                    int foldValue = 0;
                    if (arr.GetUpperBound(0) >= (line * 2 - i))
                        foldValue = arr[(line * 2 - i), j];

                    res[i, j] = arr[i, j] + foldValue;
                }
            }
            return res;
        }

        private static int[,] FoldX(int[,] arr, int line)
        {
            var res = new int[arr.GetLength(0), line];
            for (int i = 0; i < res.GetLength(0); i++)
            {
                for (int j = 0; j < res.GetLength(1); j++)
                {
                    int foldValue = 0;
                    if (arr.GetUpperBound(1) >= (line * 2 - j))
                        foldValue = arr[i,(line * 2 - j)];

                    res[i, j] = arr[i, j] + foldValue;
                }
            }
            return res;
        }
      



        public static void Run()
        {
            var lines = File.ReadLines("data/day13.txt");

            var dotCoordinats = new List<int[]>();
            var foldingInstructions = new List<(Direction, int)>();

            int[,] matrix;
            

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                if (line.Contains(','))
                {
                    dotCoordinats.Add(line.Split(',').Select(i => int.Parse(i)).Take(2).ToArray());
                }
                else {
                    var parts = line.Split("=");
                    var direction = (Direction)Enum.Parse(typeof(Direction), parts[0].Reverse().First().ToString(), true);
                    var foldingLine = int.Parse(parts[1]);
                    foldingInstructions.Add((direction, foldingLine));
                }
            }

            var xDim = dotCoordinats.Select(x => x[0]).Max();
            var yDim = dotCoordinats.Select(y => y[1]).Max();

            matrix = new int[yDim + 1, xDim + 1];

            dotCoordinats.ForEach(arr =>
                matrix[arr[1], arr[0]] = 1);

            var dotsAtStart = matrix.Cast<int>().Where(i => i > 0).ToList().Count();

            foldingInstructions.ForEach(
                fold =>
                {
                    switch (fold.Item1)
                    {
                        case Direction.y:
                            matrix = FoldY(matrix, fold.Item2);
                            break;
                        case Direction.x:
                            matrix = FoldX(matrix, fold.Item2);
                            break;                    
                    }
                    

                });
            // part one
            var dots = matrix.Cast<int>().Where(i => i > 0).Count();
            

        }


    }


}

