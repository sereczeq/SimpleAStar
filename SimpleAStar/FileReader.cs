using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SimpleAStar
{
    public static class FileReader
    {
        public static void ReadFile(string pathToFile, out List<double> nodes, out List<List<int>> connections)
        {
            var lines = File.ReadAllLines(pathToFile);

            var count = int.Parse(lines.First());

            nodes = new List<double>();
            for (var i = 1; i < count + 1; i++)
            {
                var node = float.Parse(lines[i], CultureInfo.InvariantCulture);
                nodes.Add(node);
            }

            connections = new List<List<int>>();
            for (var i = count + 1; i < count * 2 + 1; i++)
            {
                var text = lines[i];
                var list = text.Split(' ');
                var indexes = list.Select(int.Parse).ToList();
                if (indexes.First() == -1)
                {
                    indexes = new List<int>();
                }

                connections.Add(indexes);
            }
        }
    }
}