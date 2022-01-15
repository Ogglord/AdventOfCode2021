using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    public static class Day14
    {
        private static Dictionary<string, char> _translation = new Dictionary<string, char>();
        private static StringBuilder value = new StringBuilder();
        public static void Run()
        {
            var lines = File.ReadLines("data/day14.txt");
            value.Append(lines.First());
            foreach (var line in lines.Skip(2))
            {
                var pair = line.Split(" -> ");
                _translation.Add(pair[0], pair[1][0]);
            }



            for (int i = 0; i < 10; i++)
            {
                var insertions = new Dictionary<int, char>();

                for (int pos = 0; pos < value.Length-1; pos++)
                {
                    var pair = value[pos].ToString() + value[pos + 1].ToString();
                    if (_translation.ContainsKey(pair))
                        insertions.Add(pos + 1, _translation[pair]);
                }
                foreach (var item in insertions.OrderByDescending( (KeyValuePair<int, char> arg) => arg.Key))
                {
                    value.Insert(item.Key, item.Value);
                }

            }

            var elements = value.ToString().ToCharArray();
            var rankedElements = elements.GroupBy(p => p).OrderByDescending(grp => grp.Count()).Select(g => new { Element = g.Key, Count = g.Count()});
            var mostCommon = rankedElements.First();
            var leastCommon = rankedElements.Last();
            var partOne = mostCommon.Count - leastCommon.Count;
            Console.WriteLine($"Part one: {partOne}");

        }


    }


}

