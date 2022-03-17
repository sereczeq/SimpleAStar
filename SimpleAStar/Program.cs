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
        public readonly string path = @"D:\Uni\AI\SimpleAStar\SimpleAStar\SimpleAStar\";

        public readonly List<string> fileNames = new List<string>()
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

        private List<float> _nodes = new List<float> {41, 51, 50, 36, 38, 45, 21, 32, 29};
        
        private List<List<int>> _connections = new List<List<int>>
        {
            new List<int> {1, 6, 8},
            new List<int> {2},
            new List<int> {},
            new List<int> {},
            new List<int> {},
            new List<int> {3, 7},
            new List<int> {3, 7},
            new List<int> {2},
            new List<int> {4, 5}
        };

        List<float> times = Enumerable.Repeat(0f, 9).ToList();
        
        public static void Main(string[] args)
        {
            var program = new Program();
            foreach (var fileName in program.fileNames)
            {
                Console.Write(fileName + " will take: ");
                var path = program.path + fileName;
                program.ReadFile(path);
                var time = DateTime.Now;
                program.CalculateNode(0);
                var max = program.times.Max();
                Console.WriteLine(max);
                var timeDif = DateTime.Now - time;
                Console.WriteLine($"Operation took: {timeDif.Milliseconds} milliseconds ({timeDif.Ticks} ticks)");
                Console.WriteLine("---------------------------------------");
            }
           
        }


        public void ReadFile(string pathToFile)
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

            times = Enumerable.Repeat(0f, _nodes.Count).ToList();
        }
        public void CalculateNode(int index, float currentTime = 0f)
        {
            // Console.WriteLine($", current time {currentTime}...");
            var myTime = times[index];
            var myWeight = _nodes[index];
            if (myTime == 0)
            {
                times[index] = currentTime + myWeight;
            }
            else if (myTime - myWeight > currentTime)
            {
                times[index] = currentTime + myWeight;
            }
            else
            {
                // Console.WriteLine($"...Finished checking {index}, current time {currentTime}");
                return;
            }
            foreach (var child in _connections[index])
            {
                // Console.Write($"Checking {child} as child of {index}");
                CalculateNode(child, times[index]);
            }
            // Console.WriteLine($"...Finished checking {index}, current time {currentTime}");
        }

        // public IEnumerable<int> GetLastNodes()
        // {
        //     var result = new List<int>();
        //     for (var i = 0; i < _connections.Count; i++)
        //     {
        //         if (_connections[i].Count == 0)
        //         {
        //             result.Add(i);
        //         }
        //     }
        //     return result;
        // }
        //
        // public int GetMaxFromNodes(IEnumerable<int> lastNodes)
        // {
        //     return lastNodes.Select(node => times[node]).Max();
        // }
    }
}