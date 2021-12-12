using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public static class Day11
    {
      
        static A[,] ToMatrix<T, A>(this IList<T[]> arrays, Func<T, A> convertFunction)
        {
            var result = new A[arrays.Count, arrays[0].Length];
            for (var i = 0; i < arrays.Count; i++)
                for (var j = 0; j < arrays[0].Length; j++)
                    result[i, j] = convertFunction(arrays[i][j]);
            return result;
        }

      

        static ValueTuple<int, int> add(this ValueTuple<int, int> a, ValueTuple<int, int> b)
        {
            return new ValueTuple<int, int>(a.Item1 + b.Item1, a.Item2 + b.Item2);
        }

        static void TransformNeighbors<A>(this A[,] arr, ValueTuple<int, int> pos, Func<A, ValueTuple<int, int>, A> processor)
        {
            foreach (var step in new List<(int, int)> { (0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1) })
            {
                var newPos = pos.add(step);
                if (newPos.Item1 < 0 || newPos.Item2 < 0 || newPos.Item1 > arr.GetUpperBound(0) || newPos.Item2 > arr.GetUpperBound(1))
                    continue;

                arr[newPos.Item1, newPos.Item2] = processor(arr[newPos.Item1, newPos.Item2], newPos);
            }

        }

        static void TransformAll<A>(this A[,] arr, Func<A, A> processor)
        {
            for (int y = 0; y <= arr.GetUpperBound(0); y++)
                for (int x = 0; x <= arr.GetUpperBound(1); x++)
                {
                    arr[y, x] = processor(arr[y, x]);
                }
        }

        static void ForEach<A>(this A[,] arr, Action<A,int,int> action)
        {
            for (int y = 0; y <= arr.GetUpperBound(0); y++)
                for (int x = 0; x <= arr.GetUpperBound(1); x++)
                {
                    action(arr[y, x],y,x);
                }
        }

        public static void Run()
        {
            var lines = File.ReadLines("data/day11.txt");

            var matrix = lines
                .Select(l => l.ToCharArray())
                .ToList()
                .ToMatrix(x => byte.Parse(x.ToString()));

            ulong totalFlashes = 0;


            for (int i = 0; i < 1000; i++)
            {
                
                var flashQueue = new Queue<(int, int)>(); // queue to process to handle flash
                var hasFlashed = new HashSet<(int, int)>(); // list of all octopusses that has been added to queue this round
                

                // inc all by 1
                matrix.TransformAll(x => (byte)(x + 1));

                // check which flashes
                matrix.ForEach((val, y, x) =>
                {
                    if (val > 9)
                        if(hasFlashed.Add((y,x)))
                            flashQueue.Enqueue((y, x));
                });

                // do post flash processing, i.e increase neighbors +1
                while (flashQueue.Count > 0)
                {
                    // increase neighboars
                    var pos = flashQueue.Dequeue();
                    matrix.TransformNeighbors(pos, (val,valPos) =>
                    {
                        // are we making a new octopuss flash?
                        if (val == 9)
                            if (hasFlashed.Add(valPos))
                                flashQueue.Enqueue(valPos);
                        return (byte)(val + 1);
                        }
                    );
                    
                }
                totalFlashes += (ulong)Convert.ToInt64(hasFlashed.Count);
                matrix.TransformAll( val => (byte)(val > 9 ? 0 : val));

                if (hasFlashed.Count == matrix.Length)
                {
                    Console.WriteLine($"End of round {i + 1} summary: {hasFlashed.Count} octopusses flashed. All of them!. (Total {totalFlashes})");
                    break;
                }

            }
            Console.ReadKey();

        }

        
    }
}
