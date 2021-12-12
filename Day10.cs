using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public static class Day10
    {
        public class SyntaxError : Exception
        {
            private readonly char expecting;
            private readonly char invalidSyntax;
            List<(char, int)> SyntaxPoints = new List<(char, int)>{ ('}', 1197), ('>', 25137), (')', 3), (']', 57) };

            public SyntaxError(char expecting, char invalidSyntax)
            {
                this.expecting = expecting;
                this.invalidSyntax = invalidSyntax;
            }

            public int Points()
            {
                return SyntaxPoints.Single(s => s.Item1 == invalidSyntax).Item2;
            }
            public override string ToString()
            {
                return $"Expected closing for {expecting}, but found {invalidSyntax} instead";
            }

        }
        static string[] syntaxArray = new string[]{ "{}", "<>", "()", "[]" };

        public static void Run()
        {
            var lines = File.ReadLines("data/day10.txt");
            var syntaxErrorPoints = 0;
            var completionHighScore = new List<UInt64>();

            var completionPointsDict = new Dictionary<char, int>();
            completionPointsDict.Add(')', 1);
            completionPointsDict.Add(']', 2);
            completionPointsDict.Add('}', 3);
            completionPointsDict.Add('>', 4);

            foreach (var line in lines)
            {
                UInt64 completionPointsPerLine = 0;

                try
                {
                    var memory = new LinkedList<char>();
                    foreach (var ch in line)
                    {
                        var synPair = syntaxArray.Single(s => s.Contains(ch));
                        if (synPair[0] == ch)
                            // add chunk to memory
                            memory.AddLast(ch);
                        else
                            // validate closing chunk against memory
                            if (memory.Last() != synPair[0])
                            throw new SyntaxError(memory.Last(), ch);
                        else
                            memory.RemoveLast();
                    }

                    // autocomplete incomplete line
                    while (memory.Count > 0)
                    {
                        var chunk = memory.Last();
                        memory.RemoveLast();
                        var synPair = syntaxArray.Single(s => s.Contains(chunk));
                        var pointForChunk = (ulong)completionPointsDict[synPair[1]];
                        completionPointsPerLine *= 5;
                        completionPointsPerLine += pointForChunk;
                    }
                    completionHighScore.Add(completionPointsPerLine);

                }
                catch (SyntaxError sex)
                {
                    syntaxErrorPoints += sex.Points();
                    Console.WriteLine(sex);
                }
               
            }

            Console.WriteLine($"Validation completed with {syntaxErrorPoints} error(s)!");
            completionHighScore.Sort();
            var middleHighScore = completionHighScore.ElementAt((completionHighScore.Count - 1) / 2);
            Console.WriteLine($"Autocomplete completed with {middleHighScore} points!");
            Console.ReadKey();
        }

        
    }
}
