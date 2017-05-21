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


        public void ProcessMap(ChallengeMap map)
        {
            nodeListPerIndex = new Dictionary<int, List<Node>>();

            //Populating Node with Data Base on the Map
            for (int mapIndex_X = 0; mapIndex_X < map.MaxCountX; mapIndex_X++)
            {
                for (int mapIndex_Y = 0; mapIndex_Y < map.MaxCountY; mapIndex_Y++)
                {
                    Node node = new Node();

                    if (mapIndex_Y == 0)
                        node.east = 1500;
                    else
                        node.east = map.challengeMap[mapIndex_X, mapIndex_Y - 1];


                    if (mapIndex_Y == map.MaxCountY - 1)
                        node.west = 1500;
                    else
                        node.west = map.challengeMap[mapIndex_X, mapIndex_Y + 1];


                    if (mapIndex_X == map.MaxCountX - 1)
                        node.south = 1500;
                    else
                        node.south = map.challengeMap[mapIndex_X + 1, mapIndex_Y];


                    if (!nodeListPerIndex.ContainsKey(mapIndex_X))
                        nodeListPerIndex.Add(mapIndex_X, new List<Node>() { node });
                    else
                        nodeListPerIndex[mapIndex_X].Add(node);

                }
            }
        }
    }
}
