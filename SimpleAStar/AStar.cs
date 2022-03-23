using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SimpleAStar
{
    public static class AStar
    {
        public static IEnumerable<Node> SetupNodes(IEnumerable<double> nodesNotSetUp,
            List<List<int>> connectionsNotSetUp)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Set up the lists first (create new parent and end)
            var nodesAsList = AddParentAndLastNode(nodesNotSetUp);
            var connections = SetUpConnections(connectionsNotSetUp);

            // Change two lists into list of Nodes
            var nodes = CreateNodes(nodesAsList, connections);

            stopWatch.Stop();
            Console.WriteLine($"Setup of nodes took {stopWatch.Elapsed}");
            stopWatch.Reset();

            return nodes;
        }

        public static void CalculateHeuristics(IEnumerable<Node> nodes, HeuristicEnum heuristic)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Calculate Heuristic for all nodes
            foreach (var node in nodes)
            {
                node.CalculateHeuristic(heuristic);
            }

            stopWatch.Stop();
            Console.WriteLine($"Calculating heuristics took {stopWatch.Elapsed}");
        }

        public static List<Node> GetPath(Node parentNode)
        {
            var open = new List<Node>();

            parentNode.G = parentNode.MyWeight;
            open.Add(parentNode);
            var node = open[0];

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            while (node.HasNext())
            {
                // Find the Node with biggest F
                node = open[0];
                foreach (var n in open)
                {
                    if (n.F > node.F) continue;
                    node = n;
                }
                
                open.Remove(node);

                foreach (var child in node.Children)
                {
                    var currentTime = node.G + node.MyWeight;
                    
                    // If the time to get to child is smaller than current, continue checking, this is not the one
                    if (currentTime < child.G) continue;
                    child.Parent = node;
                    child.G = currentTime;
                    if(open.Contains(child)) continue;
                    open.Add(child);
                }
            }



            
            stopWatch.Stop();
            Console.WriteLine($"Traversing took {stopWatch.Elapsed}");

            
            // Create the path, going from the last node and it's parents
            var path = new List<Node>();
            while (node.Parent != null)
            {
                path.Add(node);
                node = node.Parent;
            }
            path.Reverse();
            
            return path;
        }

        private static IEnumerable<Node> CreateNodes(IEnumerable<double> nodesAsList, IList<List<int>> connections)
        {
            var nodes = nodesAsList.Select((t, i) => new Node(i, t)).ToList();

            for (var i = 0; i < connections.Count; i++)
            {
                var connection = connections[i];
                foreach (var index in connection)
                {
                    nodes[i].AddChild(nodes[index]);
                }
            }

            return nodes;
        }


        private static List<List<int>> SetUpConnections(List<List<int>> connections)
        {
            // First creating a master parent, then a master end,
            // So parent is second to last index
            // Child is last index
            connections = ConnectAllEndToNewEnd(connections);
            connections = ConnectAllParentsToNewParent(connections);
            return connections;
        }


        private static List<List<int>> ConnectAllParentsToNewParent(List<List<int>> connections)
        {
            // Create list filled with zeros
            var allNodes = Enumerable.Repeat(0, connections.Count).ToList();

            // Find all nodes without parent (they are parents)
            foreach (var connection in connections)
            {
                foreach (var index in connection.Where(index => index != -1))
                {
                    // If anyone points to this index, node at this index is not a parent, mark this node as 1
                    allNodes[index] = 1;
                }
            }

            // The connections that new master parent will have (it's children)
            var parentConnections = new List<int>();

            // Add all nodes that weren't marked (don't have the 1)
            for (var i = 0; i < allNodes.Count; i++)
            {
                if (allNodes[i] != 0) continue;
                parentConnections.Add(i);
            }

            // Add this connection to the end of the array, creating a list of children for the parent
            connections.Add(parentConnections);

            return connections;
        }

        private static List<List<int>> ConnectAllEndToNewEnd(List<List<int>> connections)
        {
            var lastNodeIndex = connections.Count;

            for (var i = 0; i < connections.Count; i++)
            {
                // If does not have any connections (any children) then it's last
                if (connections[i].Count != 0) continue;

                // Create a connection to the actual last node
                connections[i] = new List<int> {lastNodeIndex};
            }

            connections.Add(new List<int>());
            return connections;
        }

        private static IEnumerable<double> AddParentAndLastNode(IEnumerable<double> nodesNotSetUp)
        {
            return new List<double>(nodesNotSetUp) {0, 0};
        }
    }
}