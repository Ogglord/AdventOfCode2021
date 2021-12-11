using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public static class Day7
    {
       

        public static void Run()
        {
            var lines = File.ReadLines("data/day7.txt");
            var linesAsInt = lines.First().Split(',',StringSplitOptions.RemoveEmptyEntries).Select(l => int.Parse(l));

            var crabRegister = new List<int>(new int[linesAsInt.Max()+1]);
            

            int calculateFuel(int pos)
            {
                var fuelList = new List<int>(new int[linesAsInt.Max() + 1]);
                for (int i = 0; i < crabRegister.Count(); i++)
                {
                    // arithmetic sum with increase of 1 is defined as follows:
                    //   (x/2)*(2+(x-1))
                    float distance = Math.Abs(i - pos);
                    int fuel = distance == 1 ? 1 : Convert.ToInt32(distance / 2 * (2 + (distance - 1)));
                    fuelList[i] = fuel * crabRegister[i];

                }
                return fuelList.Sum();
            }

            linesAsInt.ToList().ForEach(l => crabRegister[l]++);

            var lowestFuel = int.MaxValue;
            var bestPos = -1;
            for(int i = 0; i < crabRegister.Count(); i++)
            {
                var fuel = calculateFuel(i);
                if (lowestFuel > fuel)
                {
                    lowestFuel = fuel;
                    bestPos = i;
                }
            }

            Console.WriteLine($"Best pos is {bestPos} using {lowestFuel} fuel");
           
            

        }

        
    }
}
