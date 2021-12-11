using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{

    public static class Day8
    {
        // what we know already:
        // 1 = 2 char
        // 4 = 4 char
        // 7 = 3 char
        // 8 = 7 char

        // 2,3,5 = 5 char <-- needs to be deduced for each line
        // 0,6,9 = 6 char <-- needs to be deduced for each line

        /// <summary>
        /// Maps all SIGNAL combinations to their actual values
        /// Keys are always inserted sorted alphabetically
        /// E.g "cd" to 1
        /// </summary>
        static SortedDictionary<string, int> translation = new SortedDictionary<string, int>();

        
        public static void Run()
        {
            var lines = File.ReadLines("data/day8.txt");
            var parsed = from l in lines
                     where l.Length > 0
                     select new
                     {
                         // We want all strings sorted alphabetically, it makes comparison easier.. so why not?
                         InputSorted = l.Split('|', sso).FirstOrDefault().Split(' ', sso).Select(SortString).ToList(),                         
                         OutputSorted = l.Split('|', sso).LastOrDefault().Split(' ', sso).Select(SortString).ToList()
                     };


            // selects all SIGNAL combinations of a certain length
            List<string> selectByLength(List<string> signals, int length)
            {               
                var actualKey = signals.Where(o => o.Length == length).Distinct().ToList();
                return actualKey;
            }

            // adds a SIGNAL => value to our dictionary
            void map(List<string> signals, int length, int actualValue)
            {
                var actualKey = selectByLength(signals, length);
                if (actualKey.Count() == 1)
                    translation.Add(actualKey.Single(), actualValue);

            }

            /// returns the SIGNAL combination for a certain value, i.e "eg" for 1
            string getSignalsFor(int value) {
                return translation.Where(kvp => kvp.Value == value).Select( kvp => kvp.Key).Single();
            }

            // parse each line, build dictionary for each and calculate OUTPUT
            int sumOfOutput = 0;
            foreach (var line in parsed)
            {
                Console.Write($"{string.Join(' ',line.InputSorted)} | {string.Join(' ', line.OutputSorted)}");

                // merge input and output into a single list so we can use all values to deduct the wiring...
                var allSignals = new List<string>(line.InputSorted);
                allSignals.AddRange(line.OutputSorted);

                // we know 1,4,7,8 so map them first
                map(allSignals, length: 2, actualValue: 1);
                map(allSignals, length: 4, actualValue: 4);
                map(allSignals, length: 3, actualValue: 7);
                map(allSignals, length: 7, actualValue: 8);

                // get top line SIGNAL as 7 - 1
                var topSignal = getSignalsFor(7).subtract(getSignalsFor(1));
                               
                // 8 - (1+4+7) gives us bottom+bottomRight SIGNALS
                var bottomBottomRightSignal = getSignalsFor(8)
                    .subtract(getSignalsFor(1))
                    .subtract(getSignalsFor(4))
                    .subtract(getSignalsFor(7));

                // select 0,6,9 together, take the highest the common denominator, then subtract 1+4+7
                // this provides us the BOTTOM SIGNAL
                var groupBy = selectByLength(allSignals, 6).SelectMany(s => s.ToCharArray()).GroupBy(c => c);
                var mostCommon069 = groupBy.Where(grp => grp.Count() == groupBy.Select(g => g.Count()).Max()).Select( g => g.Key);
                var bottomSignal = new string(mostCommon069.ToArray())
                    .subtract(getSignalsFor(1))
                    .subtract(getSignalsFor(4))
                    .subtract(getSignalsFor(7));

                // lets call mid+topleft gamma, we know it as 4 - 1
                var gamma = getSignalsFor(4)
                            .subtract(getSignalsFor(1));
                // use same approach as for BOTTOM SIGNAL to derive MIDDLE SIGNAL
                var middle = gamma
                    .subtract(new string(mostCommon069.ToArray()));
                // we have gamma left part, or upper part of left signal
                var gammaLeft = gamma.subtract(middle);

                // beta is what I call lower part of left signal
                var beta = bottomBottomRightSignal
                    .subtract(bottomSignal);

                // ypsilon is what I call both right signals (i.e. signals for ONE), we split into TOP and BOT
                var ypsilonTop = getSignalsFor(1)
                    .subtract(new string(mostCommon069.ToArray()));
                var ypsilonBot = getSignalsFor(1).subtract(ypsilonTop);

                // now we can complete the dictionary
                translation.Add(SortString("abcdefg".subtract(middle)), 0);
                translation.Add(SortString(topSignal + bottomSignal + middle + ypsilonTop + beta), 2);
                translation.Add(SortString(topSignal + bottomSignal + middle + ypsilonTop + ypsilonBot), 3);
                translation.Add(SortString(topSignal + bottomSignal + middle + gammaLeft + ypsilonBot), 5);
                translation.Add(SortString("abcdefg".subtract(ypsilonTop)), 6);
                translation.Add(SortString("abcdefg".subtract(beta)), 9);


                // calculate output values
                var thisOutput = new System.Text.StringBuilder();
                
                foreach (var o in line.OutputSorted)
                    thisOutput.Append(translation[o]);

                Console.WriteLine($" => {thisOutput}");
                sumOfOutput += int.Parse(thisOutput.ToString());
                translation.Clear();

            }

            Console.WriteLine($"Total output sum: {sumOfOutput}");
            Console.ReadKey();
            

        }


        static StringSplitOptions sso = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;

        /// <summary>
        /// Sorts a strings characters alphabetically "acb" => "abc"
        /// </summary>
        /// <param name="input">unsorted</param>
        /// <returns>sorted</returns>
        static string SortString(string input)
        {
            char[] characters = input.ToArray();
            Array.Sort(characters);
            return new string(characters);
        }

        /// <summary>
        ///  E.g. removes "ab" from "abc" and returns "c"
        /// </summary>
        /// <param name="pattern1">string 1</param>
        /// <param name="pattern2">string 2</param>
        /// <returns>pattern1 with all characters from pattern2 removed</returns>
        static string subtract(this string pattern1, string pattern2)
        {
            var result = pattern1;
            foreach (char c in pattern2.ToArray())
                result = result.Replace(c.ToString(), "");
            return result;

        }

    }
}
