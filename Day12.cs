using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    public static class Day12
    {
        /// <summary>
        /// Nice function to have, checks if a cave (string) is small or big :)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static bool IsSmallCave(this string s)
        {
            return char.IsLower(s[0]);
        }

        /// <summary>
        /// Immutable struct that represents a specific path
        /// </summary>
        struct Path
        {
            public int Count { get { return visitedNodes.Count; } }
            public string CurrentCave { get { return visitedNodes.Last(); } }

            private readonly LinkedList<string> visitedNodes;

     
            public Path(string startNode)
            {
                visitedNodes = new LinkedList<string>();
                visitedNodes.AddLast(startNode);                
            }

            private Path(string newNode, IEnumerable<string> visitedNodes)
            {
                this.visitedNodes = new LinkedList<string>(visitedNodes);
                this.visitedNodes.AddLast(newNode);
            }

            public Path AddNode(string node)
            {
                
                return new Path(node, this.visitedNodes.AsEnumerable());
            }
            
            /// <summary>
            /// Returns true if the path can continue to the node, false otherwise
            /// </summary>
            /// <param name="node"></param>
            /// <returns>true/false</returns>
            public bool CanVisit(string node)
            {
                if (node.IsSmallCave())
                {
                    if (node == "start")
                        return false;
                    if (!visitedNodes.Contains(node))
                        return true;
                    
                    //we have visited this small cave before, it is fine to visit one more small cave so lets check...
                    var freePassUsed = visitedNodes.Where(n => n.IsSmallCave()).GroupBy(n => n).Any(grp => grp.Count() == 2);
                    return !freePassUsed;
                }
                return true;
            }

            public override string ToString()
            {
                var result = new StringBuilder();
                foreach (var node in visitedNodes)
                    result.Append($"{node},");
                return result.ToString();
            }


        }

        public static void Run()
        {
            var lines = File.ReadLines("data/day12.txt");

            var nodeConnections = new Dictionary<string, List<string>>();

            // build node connections as dictionary
            lines
                .ToList()
                .ForEach(
                row =>
                {
                    var pair = row.Split("-");
                  
                    if (!nodeConnections.ContainsKey(pair[0]))
                        nodeConnections.Add(pair[0], new List<string>());
                    if (!nodeConnections.ContainsKey(pair[1]))
                        nodeConnections.Add(pair[1], new List<string>());
                    nodeConnections[pair[0]].Add(pair[1]);
                    nodeConnections[pair[1]].Add(pair[0]);

                });


        
            var activePaths = new Queue<Path>();
            var completedPaths = new List<Path>();

            // evaluate all paths until they have reached the end cave
            activePaths.Enqueue(new Path("start"));
            while (activePaths.Count > 0)
            {
                var currentPath = activePaths.Dequeue();
                if (currentPath.CurrentCave == "end")
                {
                    // this path is completed
                    completedPaths.Add(currentPath);
                    continue;
                }

                // explore all connecting caves, add them to active paths
                var viableOptions = nodeConnections[currentPath.CurrentCave].Where(currentPath.CanVisit);
                var newPath = viableOptions.Select(currentPath.AddNode);
                newPath.ToList().ForEach(p => activePaths.Enqueue(p));

            }

            completedPaths.Sort( (a,b) => a.ToString().CompareTo(b.ToString()));
            Console.WriteLine($"Found a total of {completedPaths.Count} unique paths from start to end");
        }

        
    }
}
