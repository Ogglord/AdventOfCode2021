using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public static class Day3
    {
        static int aim = 0;
        static int x = 0;
        static int y = 0; 

        public static void Run()
        {
            var lines = File.ReadLines("data/day3.txt");
            var listOfInts = lines
                .Select<string, List<int>>(x => x
                            .ToCharArray()
                            .Select(c => int.Parse(c.ToString()))
                            .ToList()
                );

            string gamma = string.Empty;
            string epsilon = string.Empty;

            for (int i = 0; i < 12; i++)
            {
                var bits = listOfInts.Select(l => l[i]);
                if (bits.Sum() >= 500)
                {
                    gamma += "1";
                    epsilon += "0";
                }
                else
                {
                    gamma += "0";
                    epsilon += "1";
                }
            }
            int gammaDecimal = Convert.ToInt32(gamma, 2);
            int epsilonDecimal = Convert.ToInt32(epsilon, 2);
            int result = gammaDecimal * epsilonDecimal;


            // part two
            //oxygen
            var oxygenCandidates = listOfInts.ToList();
            int x = 0;
            while (oxygenCandidates.Count() > 1)
            {
                oxygenCandidates = oxygenCandidates.Where(l => l[x] == oxygenBit(oxygenCandidates, x)).ToList();

                x++;
            }
            var co2Candidates = listOfInts.ToList();
            x = 0;
            while (co2Candidates.Count() > 1)
            {

                co2Candidates = co2Candidates.Where(l => l[x] == co2Bit(co2Candidates, x)).ToList();

                x++;
            }
            var oxygenBinary = String.Join("", oxygenCandidates.Single().Select(j => j.ToString()).ToArray());
            var oxygen = Convert.ToInt32(oxygenBinary, 2);

            var co2Binary = String.Join("", co2Candidates.Single().Select(j => j.ToString()).ToArray());
            var co2 = Convert.ToInt32(co2Binary, 2);

            var result2 = oxygen * co2;

        }

        static int oxygenBit(List<List<int>> values, int index)
        {
            var bits = values.Select(l => l[index]);

            var zeroes = bits.Count(v => v == 0);
            var ones = bits.Count(v => v == 1);
            if (zeroes == ones || ones > zeroes)
                return 1;
            else
                return 0;

        }

        static int co2Bit(List<List<int>> values, int index)
        {
            var bits = values.Select(l => l[index]);

            var zeroes = bits.Count(v => v == 0);
            var ones = bits.Count(v => v == 1);
            if (zeroes == ones || ones > zeroes)
                return 0;
            else
                return 1;

        }
    }
}
