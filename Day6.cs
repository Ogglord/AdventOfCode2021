using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public static class Day6
    {


        public static void Run()
        {
            Console.WriteLine("Welcome to Day6 part two...");
            var lines = File.ReadLines("data/day6.txt");
            var startingFish = lines.SelectMany(l => l.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => byte.Parse(s)));

            var fishArr = new long[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            void printFish(int day)
            {
                Console.WriteLine($"Day {day}: {String.Join(", ", fishArr)} (Total fish: {fishArr.Sum()})");
            }

            // create start pop
            foreach (var f in startingFish)
                fishArr[f]++;

            printFish(0);

            for (int i = 0; i < 256; i++)
            {

                var spawning = fishArr.Pop();
                fishArr[6] += spawning;
                fishArr[8] = spawning;

                printFish(i + 1);

            }
            Console.ReadKey();

        }

        public static T Pop<T>(this T[] arr)
        {
            var shifts = 1;
            var result = arr[0];

            Array.Copy(arr, shifts, arr, 0, arr.Length - shifts);
            Array.Clear(arr, arr.Length - shifts, shifts);

            return result;
        }

      


    }
}
