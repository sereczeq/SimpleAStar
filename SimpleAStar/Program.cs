using System;
using System.Collections.Generic;
using System.Data;
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
            // HeuristicEnum.Random,
            HeuristicEnum.BruteForce,

            // HeuristicEnum.SmallestWeight,
            // HeuristicEnum.CountUntilEnd,
        };

        public static void Main(string[] args)
        {
            foreach (var fileName in FileNames)
            {
                var filePath = Path + fileName;
                FileReader.ReadFile(filePath, out var nodes, out var connections);

                var setupNodes = AStar.SetupNodes(nodes, connections).ToList();

                foreach (var heuristic in HeuristicEnums)
                {
                    AStar.CalculateHeuristics(setupNodes, heuristic);
                    var path = AStar.GetPath(setupNodes.Last());

                    DebugInfo(fileName, heuristic, path);
                }
            }
        }

        private static void DebugInfo(string fileName, HeuristicEnum heuristic, List<Node> path,
            bool detail = false)
        {
            var debug = $"For file {fileName} and heuristic {heuristic}";
            var totalTime = path.Sum(node => node.MyWeight);
            if (detail)
            {
                debug += ", the path is\n";
                foreach (var node in path)
                {
                    debug += node + "\n";
                }

                debug += $"With total weight of {totalTime}";
            }
            else
            {
                debug += $" the total time is {totalTime}";
            }
            debug += "\n------------------------------------------------\n";
            Console.WriteLine(debug);
        }
    }
}