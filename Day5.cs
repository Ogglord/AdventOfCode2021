using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public static class Day5
    {


        public static void Run()
        {
            var lines = File.ReadLines("data/day5.txt");
            var vectorQuery = from line in lines
                              select new
                              {
                                  From = cord.FromString(line.Split(" -> ").First()),
                                  To = cord.FromString(line.Split(" -> ").Last())
                              };


            var maxY = vectorQuery.SelectMany(c => new int[] { c.From.Y, c.To.Y }).Max();
            var maxX = vectorQuery.SelectMany(c => new int[] { c.From.X, c.To.X }).Max();

            var diagram = new int[maxX + 1, maxY + 1];

            void printDiagram()
            {
                for (int y = 0; y <= maxY; y++)
                {
                    for (int x = 0; x <= maxX; x++)
                        Console.Write(diagram[x, y] == 0 ? "." : diagram[x, y]);
                    Console.WriteLine();
                }
            }

            void plotVector(cord fr, cord to)
            {
                Console.WriteLine($"Plotting from {fr} to {to}");
                //Console.ReadKey();
                var x1 = fr.X;
                var x2 = to.X;
                var y1 = fr.Y;
                var y2 = to.Y;


                var incY = (y2 - y1);
                var incX = (x2 - x1);
                var steps = Math.Max(Math.Abs(incY), Math.Abs(incX)) + 1;
                              

                for(int i = 0; i < steps; i++)
                {
                    diagram[x1, y1]++;

                    x1 += Math.Sign(incX);
                    y1 += Math.Sign(incY);
                }

                //printDiagram();
                
            }

            vectorQuery
                //.Where(p => p.From.X == p.To.X || p.From.Y == p.To.Y) // skip diagonal
                .ToList()
                .ForEach(x => plotVector(x.From,x.To));

            printDiagram();

            var intersections = diagram.Cast<int>().Where(d => d > 1).Count();

            Console.WriteLine($"Finnished. There are {intersections} intersections (> 2)...");
            Console.ReadKey();
        }



        struct cord
        {
            public int X { get; set; }
            public int Y { get; set; }

            public cord(int x, int y) : this()
            {
                Y = y;
                X = x;
            }

            public static cord FromString(string cordStr)
            {
                var pair = cordStr.Split(',');

                return new cord(int.Parse(pair[0]), int.Parse(pair[1]));
            }

            public override string ToString()
            {
                return $"{X},{Y}";
            }
        }
    }
}
