using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleAStar
{
    public class Node
    {
        private readonly int _index;
        
        public double MyWeight { get; }

        public double G;

        private double _h;


        public double F => G + _h;

        private readonly List<Node> _parents;

        public readonly List<Node> Children;
        public Node Parent;

        public Node(int index, double myWeight)
        {
            _index = index;
            MyWeight = myWeight;
            _parents = new List<Node>();
            Children = new List<Node>();
        }

        public void AddChild(Node child)
        {
            Children.Add(child);
            child._parents.Add(this);
        }

        public override string ToString()
        {
            return $"Node {_index}, {MyWeight:F15}, g = {G:F15}, h = {_h:F15}, F = {F:F15}";
        }

        public bool HasNext()
        {
            return Children.Count != 0;
        }

        public void CalculateHeuristic(HeuristicEnum heuristicEnum)
        {
            switch (heuristicEnum)
            {
                case HeuristicEnum.BruteForce:
                    BruteForceHeuristic();
                    break;
                case HeuristicEnum.CountUntilEnd:
                    CountUntilEndHeuristic();
                    break;
                case HeuristicEnum.SmallestWeight:
                    SmallestGHeuristic();
                    break;
                case HeuristicEnum.Random:
                default:
                    RandomHeuristic();
                    break;
            }
        }

        // This one can only be fired by the top node
        private void BruteForceHeuristic()
        {
            if(_parents.Count != 0) return;
            DepthFirst(0);
        }

        // private double BruteForce()
        // {
        //     if (_h == 0)
        //     {
        //         _h = MyWeight;
        //     }
        //
        //     // h += children.Select(child => child.BruteForce()).Max();
        //
        //     var childWeights = new List<double>();
        //     foreach (var child in Children)
        //     {
        //         var childWeight = child.BruteForce();
        //         childWeights.Add(childWeight);
        //     }
        //
        //     if (childWeights.Count != 0)
        //     {
        //         _h += childWeights.Max();
        //     }
        //     return _h;
        // }

        private void DepthFirst(double currentWeight)
        {
            if (_h == 0)
            {
                _h = currentWeight + MyWeight;
            }
            else if (_h - MyWeight < currentWeight)
            {
                _h = currentWeight + MyWeight;
            }
            else
            {
                return;
            }

            foreach (var child in Children)
            {
                child.DepthFirst(_h);
            }
        }

        private void CountUntilEndHeuristic()
        {
            _h = CountDepth();
        }

        private int CountDepth()
        {
            if (Children.Count == 0) return 1;
            return 1 + Children.First().CountDepth();
        }

        private void SmallestGHeuristic()
        {
            _h = MyWeight;
        }

        private void RandomHeuristic()
        {
            var random = new Random(Children.Count + _index * DateTime.Now.Millisecond);
            _h = random.NextDouble();
        }
    }
}