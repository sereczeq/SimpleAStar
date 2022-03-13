using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SimpleAStar
{
    internal class Program
    {
        List<int> nodes = new List<int> {41, 51, 50, 36, 38, 45, 21, 32, 29};
        List<List<int>> connections = new List<List<int>>
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

        List<int> times = Enumerable.Repeat(0, 9).ToList();
        
        public static void Main(string[] args)
        {
            var program = new Program();
            // var result = program.FindTime();
            // Console.WriteLine(result);
            
            program.CalculateNode(0);
            var last = program.GetLastNodes();
            var max = program.GetMaxFromNodes(last);
            Console.WriteLine(max);




        }

        public int FindTime()
        {
            for (var i = 0; i < nodes.Count; i++)
            {
                Console.WriteLine($"Starting checking node number {i}...");
                foreach (var child in connections[i])
                {
                    Console.WriteLine($"Checking child {child}");
                    var currentTimeToGetToChild = times[child];
                    var myTimeToGetToChild = times[i] + nodes[i];
                    Console.WriteLine($"Current child time {currentTimeToGetToChild}, My child time {myTimeToGetToChild}");
                    if (myTimeToGetToChild < currentTimeToGetToChild || currentTimeToGetToChild == 0)
                    {
                        times[child] = myTimeToGetToChild;
                    }
                    Console.WriteLine($"Node {child} has time {times[child]}");
                }
                Console.WriteLine($"...Finished checking node number {i}");
            }

            for (var i = 0; i < nodes.Count; i++)
            {
                times[i] += nodes[i];
            }

            return times.Max();
        }

        public void CalculateNode(int index, int currentTime = 0)
        {
            var myTime = times[index];
            var myWeight = nodes[index];
            Console.WriteLine($"Node {index} with weight {myWeight} currently has time {myTime}");
            if (myTime == 0)
            {
                times[index] = currentTime + myWeight;
            }
            else if (myTime - myWeight > currentTime)
            {
                times[index] = currentTime + myWeight;
            }
            Console.WriteLine($"Node {index} with weight {myWeight} now has time {times[index]}");
            foreach (var child in connections[index])
            {
                CalculateNode(child, times[index]);
            }
        }

        public List<int> GetLastNodes()
        {
            var result = new List<int>();
            for (int i = 0; i < connections.Count; i++)
            {
                if (connections[i].Count == 0)
                {
                    result.Add(i);
                }
            }
            return result;
        }

        public int GetMaxFromNodes(IEnumerable<int> lastNodes)
        {
            return lastNodes.Select(node => times[node]).Max();
        }
    }
}