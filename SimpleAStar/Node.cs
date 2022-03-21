using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleAStar
{
    public class Node
    {
        private readonly int _index;
        
        public double MyWeight { get; }

        private double _g;

        private double _h;


        private double F => _g + _h;

        private readonly List<Node> _parents;

        private readonly List<Node> _children;

        public Node(int index, double myWeight)
        {
            _index = index;
            MyWeight = myWeight;
            _parents = new List<Node>();
            _children = new List<Node>();
        }

        public void AddChild(Node child)
        {
            _children.Add(child);
            child._parents.Add(this);
        }

        public override string ToString()
        {
            return $"Node {_index}, g = {_g}, h = {_h}, F = {F}";
        }

        public bool HasNext()
        {
            return _children.Count != 0;
        }

        public Node Traverse()
        {
            if (!HasNext())
            {
                throw new IndexOutOfRangeException();
            }
            var maxNode = _children.First();
            foreach (var child in _children)
            {
                if (child._g < _g)
                {
                    child._g = _g;
                }
                if (child.F < maxNode.F) continue;
                maxNode = child;
            }
            
            return maxNode;
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
            _h = BruteForce();
        }

        private double BruteForce()
        {
            if (_h == 0)
            {
                _h = MyWeight;
            }

            // h += children.Select(child => child.BruteForce()).Max();

            var childWeights = new List<double>();
            foreach (var child in _children)
            {
                var childWeight = child.BruteForce();
                childWeights.Add(childWeight);
            }

            if (childWeights.Count != 0)
            {
                _h += childWeights.Max();
            }
            return _h;
        }

        private void CountUntilEndHeuristic()
        {
            _h = CountDepth();
        }

        private int CountDepth()
        {
            if (_children.Count == 0) return 1;
            return 1 + _children.First().CountDepth();
        }

        private void SmallestGHeuristic()
        {
            _h = MyWeight;
        }

        private void RandomHeuristic()
        {
            var random = new Random(_children.Count + _index * DateTime.Now.Millisecond);
            _h = random.NextDouble();
        }
    }
}