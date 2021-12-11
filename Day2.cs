using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public static class Day2
    {
        public class MoveVector
        {
            public MoveVector(string x)
            {
                //forward 3
                var strDirection = x.Split(' ').First();
                var strLength = x.Split(' ').Last();
                Direction = (MoveDirection)Enum.Parse(typeof(MoveDirection), strDirection);
                Length = int.Parse(strLength);
            }

            public MoveDirection Direction { get; set; }
            public int Length { get; set; }
        }

        public enum MoveDirection
        {
            forward,
            down,
            up
        }

        static int aim = 0;
        static int x = 0;
        static int y = 0;

        public static void Run()
        {
            var lines = File.ReadLines("data/day2.txt");
            var lineObj = lines.Select<string, MoveVector>(x => new MoveVector(x)).ToList();
            Console.WriteLine($"{lineObj.Count} vectors loaded...");
            // Part One
            //lineObj.ForEach(ProcessVector);

            // Part Two
            lineObj.ForEach(ProcessVector2);


            Console.WriteLine($"x:{x}, y:{y} => x*y:{x * y}");
        }

        static void ProcessVector(MoveVector m)
        {
            switch (m.Direction)
            {
                case MoveDirection.forward:
                    x += m.Length;
                    break;
                case MoveDirection.down:
                    y += m.Length;
                    break;
                case MoveDirection.up:
                    y -= m.Length;
                    break;
                default:
                    Console.WriteLine("Invalid MoveDirection...");
                    break;
            }
        }

        static void ProcessVector2(MoveVector m)
        {
            switch (m.Direction)
            {
                case MoveDirection.forward:
                    x += m.Length;
                    y += aim * m.Length;
                    break;
                case MoveDirection.down:
                    aim += m.Length;
                    break;
                case MoveDirection.up:
                    aim -= m.Length;
                    break;
                default:
                    Console.WriteLine("Invalid MoveDirection...");
                    break;
            }
        }

    }
}

