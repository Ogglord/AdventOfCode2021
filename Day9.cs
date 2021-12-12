using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;


namespace AdventOfCode
{
    public static class Day9
    {
        static byte[,] heightmap;

        static A[,] ToRectArray<T, A>(this IList<T[]> arrays, Func<T, A> convertFunction)
        {
            var result = new A[arrays.Count, arrays[0].Length];
            for (var i = 0; i < arrays.Count; i++)
                for (var j = 0; j < arrays[0].Length; j++)
                    result[i, j] = convertFunction(arrays[i][j]);
            return result;
        }


        /// <summary>
        /// returns true if strictly smaller than value of x y coordinate (or x,y are out of range)
        /// </summary>
        /// <param name="valueToCompare"></param>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        static bool comparePoint(this byte valueToCompare, int y, int x)
        {
            Debug.WriteLine($"Comparing {valueToCompare} to y{y} x{x}...");
            if (x < 0 || y < 0 || y > heightmap.GetUpperBound(0) || x > heightmap.GetUpperBound(1))
                return true;
            var result = heightmap[y, x];
            return valueToCompare < result;

        }

        static ValueTuple<int, int> add(this ValueTuple<int, int> a, ValueTuple<int, int> b)
        {
            return new ValueTuple<int, int>(a.Item1 + b.Item1, a.Item2 + b.Item2);
        }

        static IEnumerable<(int, int)> findNeighbors(ValueTuple<int,int> pos)
        {
            //Debug.WriteLine($"Comparing {valueToCompare} to y{y} x{x}...");
            
            foreach (var step in new List<(int, int)>{ (0,1), (1, 0) , (-1, 0) , (0, -1) })
            {
                var newPos = pos.add(step);
                if (newPos.Item1 < 0 || newPos.Item2 < 0 || newPos.Item1 > heightmap.GetUpperBound(0) || newPos.Item2 > heightmap.GetUpperBound(1))
                    continue;
                var valueAtNewPos = heightmap[newPos.Item1, newPos.Item2];
                if (valueAtNewPos == 9)
                    continue;
                yield return newPos;
            }
            
           

        }

        public static void Run()
        {
            var lines = File.ReadAllLines("data/day9.txt");

            heightmap = lines
            .Select(l => l.ToCharArray())
            .ToList()
            .ToRectArray( x => byte.Parse(x.ToString()));


            // stores lowpoints as y,x coordinates
            var lowPointCoordinates = new List<(int, int)>();


            for (int y = 0; y <= heightmap.GetUpperBound(0); y++)
                for (int x = 0; x <= heightmap.GetUpperBound(1); x++)
                {
                    //Console.WriteLine($"Comparing {heightmap[y, x]} at row {y} and column {x}...");
                    if (heightmap[y, x].comparePoint(y - 1, x))
                        if (heightmap[y, x].comparePoint(y, x - 1))
                            if (heightmap[y, x].comparePoint(y + 1, x))
                                if (heightmap[y, x].comparePoint(y, x + 1))
                                {
                                    //Console.WriteLine($"Found lowpoint at x{x}, y{y} with value of {heightmap[y, x]}");
                                    lowPointCoordinates.Add((y, x));
                                }
                                   

                }

           
            Dictionary<(int, int), int> basinSize = new Dictionary<(int, int), int>();

            //explore basins of every lowpoint
            
            foreach (var lowPoint in lowPointCoordinates)
            {
                var frontier = new Queue<(int, int)>();
                var explored = new HashSet<(int, int)>();
               
                frontier.Enqueue(lowPoint);
                explored.Add(lowPoint);
                while (frontier.Count > 0)
                {
                    var currentPos = frontier.Dequeue();
                    
                    foreach (var nbor in findNeighbors(currentPos))
                    {
                        if (explored.Add(nbor))
                        
                            frontier.Enqueue(nbor);
                    }
                }

                // end of exploring
                basinSize.Add(lowPoint, explored.Count);

            }


            var lowPointRiskMetric = lowPointCoordinates.Select(xy => heightmap[xy.Item1, xy.Item2] + 1).Sum();
            Console.WriteLine($"Found {lowPointCoordinates.Count()} lowpoints with risk score of {lowPointRiskMetric}.");

            var threeLargestBasins = basinSize.OrderByDescending(b => b.Value).Take(3);
            var product = threeLargestBasins.Aggregate(1, (a, b) => a * b.Value);

            Console.WriteLine($"Three largets basins sum to {product}");


        }

        
    }
}
