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
            var lines = File.ReadLines("data/day14_test.txt");
            value.Append(lines.First());
            foreach (var line in lines.Skip(2))
            {
                var pair = line.Split(" -> ");
                _translation.Add(pair[0], pair[1][0]);
            }

            foreach (var kvp in _translation)
            {
                value = value.Replace(kvp.Key, new string(new[]{kvp.Key[0],kvp.Value,kvp.Key[1]}));
            }

            var pairs = new List<string>();
            var valueArr = value.ToString().ToCharArray();
            for (var i = 0; i < value.Length - 1; i++)
            {
                pairs.Add(new string(new[]{valueArr[i],valueArr[i+1]}));
            }

            var pairCount = pairs.GroupBy(p => p).OrderByDescending(grp => grp.Count()).ToList();
            var mostCommon = pairs.First();
            var leastCommon = pairs.Last();

        }


    }


}

