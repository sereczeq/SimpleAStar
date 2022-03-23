using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleAStar
{
    internal static class Program
    {
        private const string Path = @"D:\Uni\AI\SimpleAStar\SimpleAStar\SimpleAStar\";

        private static readonly List<string> FileNames = new List<string>()
        {
            "default.txt",

            "test_large.dag",
            "test_large_sparse.dag",
            "test_medium.dag",

            "test_medium_sparse.dag",
            "test_small.dag",
            "test_small_sparse.dag",
            "test_xlarge.dag",
            "test_xlarge_sparse.dag",
        };


        private static readonly List<HeuristicEnum> HeuristicEnums = new List<HeuristicEnum>
        {
            HeuristicEnum.Random,
            HeuristicEnum.BruteForce,
            HeuristicEnum.SmallestWeight,
            HeuristicEnum.CountUntilEnd,
            HeuristicEnum.ChildWeights
        };

        public static void Main(string[] args)
        {
            foreach (var fileName in FileNames)
            {
                var filePath = Path + fileName;
                
                Console.WriteLine($"<File: {fileName}>");
                
                FileReader.ReadFile(filePath, out var nodes, out var connections);

                var setupNodes = AStar.SetupNodes(nodes, connections).ToList();

                foreach (var heuristic in HeuristicEnums)
                {
                    Console.WriteLine($"<Heuristic: {heuristic}>");
                    AStar.CalculateHeuristics(setupNodes, heuristic);
                    var path = AStar.GetPath(setupNodes.Last());

                    DebugInfo(path, true);
                    Console.WriteLine($"<\\Heuristic: {heuristic}>\n" +
                                      $"\n--------------------------------------------------------------\n");
                }
                
                Console.WriteLine($"<\\File: {fileName}>\n" +
                                  $"\n--------------------------------------------------------------\n" +
                                  $"\n--------------------------------------------------------------\n" +
                                  $"\n--------------------------------------------------------------\n");
            }
        }

        private static void DebugInfo(List<Node> path,
            bool detail = false)
        {
            var totalTime = path.Sum(node => node.MyWeight);
            Console.WriteLine($"Total time is {totalTime}");
            if (!detail) return;
            {
                var debug = "";
                debug += "The path is\n";
                foreach (var node in path)
                {
                    debug += node + "\n";
                }

                debug += $"With total weight of {totalTime}";
                Console.WriteLine(debug);
            }
        }
    }
}