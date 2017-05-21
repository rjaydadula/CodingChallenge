using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingChallenge
{
    public class Node
    {
        public int east { get; set; }
        public int west { get; set; }
        public int south { get; set; }
    }

    public class PathFinder
    {
        public Dictionary<int, List<Node>> nodeListPerIndex { get; set; }
        public int LengthOfCalculatedPath { get; set; }
        public int DropOfCalculatedPath { get; set; }
        public string CalculatedPath { get; set; }
    }
}
