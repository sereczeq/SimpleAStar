using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SimpleAStar
{
    internal class Program
    {
        private const string Path = @"D:\Uni\AI\SimpleAStar\SimpleAStar\SimpleAStar\";

        private readonly List<string> _fileNames = new List<string>()
        {
            "default.txt",

            // "test_large.dag",
            // "test_large_sparse.dag",
            "test_medium.dag",
            "test_medium_sparse.dag",
            "test_small.dag",
            "test_small_sparse.dag",
            "test_xlarge.dag",
            "test_xlarge_sparse.dag",
        };

        // Nodes stored as indexes and weights, for example node 2 has weight 50
        private List<float> _nodes = new List<float> {41, 51, 50, 36, 38, 45, 21, 32, 29};

        // Connections stored as indexes and Lists of indexes, for example node 8 is pointing towards nodes 4 and 5
        private List<List<int>> _connections = new List<List<int>>
        {
            new List<int> {1, 6, 8},
            new List<int> {2},
            new List<int> { },
            new List<int> { },
            new List<int> { },
            new List<int> {3, 7},
            new List<int> {3, 7},
            new List<int> {2},
            new List<int> {4, 5}
        };

        // Time it takes to pass a node, initialized as 0
        private List<float> _times = Enumerable.Repeat(0f, 9).ToList();

        public static void Main(string[] args)
        {
            var program = new Program();
            foreach (var fileName in program._fileNames)
            {
                Console.Write(fileName + " will take: ");
                var path = Program.Path + fileName;
                program.ReadFile(path);
                var time = DateTime.Now;
                program.CalculateNode(0);

                // Find the maximum time of all shortest paths (explained in report)
                var max = program._times.Max();
                Console.WriteLine(max);
                var timeDif = DateTime.Now - time;
                Console.WriteLine($"Operation took: {timeDif.Milliseconds} milliseconds ({timeDif.Ticks} ticks)");
                Console.WriteLine("---------------------------------------");
            }
        }


        private void ReadFile(string pathToFile)
        {
            var lines = File.ReadAllLines(pathToFile);

            var count = int.Parse(lines.First());

            _nodes = new List<float>();
            for (var i = 1; i < count + 1; i++)
            {
                var node = float.Parse(lines[i], CultureInfo.InvariantCulture);
                _nodes.Add(node);
            }

            _connections = new List<List<int>>();
            for (var i = count + 1; i < count * 2 + 1; i++)
            {
                var text = lines[i];
                var list = text.Split(' ');
                var connections = list.Select(int.Parse).ToList();
                if (connections.First() == -1)
                {
                    connections = new List<int>();
                }

                _connections.Add(connections);
            }

            // for (var i = 0; i < _nodes.Count; i++)
            // {
            //     var print = _nodes[i] + " => ";
            //     foreach (var connection in _connections[i])
            //     {
            //         print += connection + " ";
            //     }
            //     Console.WriteLine(print);
            // }

            _times = Enumerable.Repeat(0f, _nodes.Count).ToList();
        }

        private void CalculateNode(int index, float currentTime = 0f)
        {
            // Console.WriteLine($", current time {currentTime}...");
            var myTime = _times[index];
            var myWeight = _nodes[index];

            // If it's the first time visiting the node, just assign it current time + it's time
            if (myTime == 0)
            {
                _times[index] = currentTime + myWeight;
            }

            // If it's not the first time visiting the node, compare if current way to get to it is better
            else if (myTime - myWeight > currentTime)
            {
                // if current way is better, update the time and proceed to check children
                _times[index] = currentTime + myWeight;
            }

            // If the current way is not better, don't check the node anymore
            else
            {
                // Console.WriteLine($"...Finished checking {index}, current time {currentTime}");
                return;
            }

            // Check all children of the node
            foreach (var child in _connections[index])
            {
                // Console.Write($"Checking {child} as child of {index}");
                CalculateNode(child, _times[index]);
            }

            // Console.WriteLine($"...Finished checking {index}, current time {currentTime}");
        }
    }
}