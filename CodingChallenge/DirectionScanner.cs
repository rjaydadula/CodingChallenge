using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingChallenge
{
    public class Direction
    {
        public const int Left = 1;
        public const int Right = 2;
        public const int Down = 3;
    }
    public class DirectionScanner
    {

       public List<List<PathNode>> pathNodeInTotalSearched { get; set; } = new List<List<PathNode>>();
       public List<PathNode> avoidNodeList { get; set; } = new List<PathNode>();
       public HashSet<PathNode> bottleNeckList { get; set; } = new HashSet<PathNode>();
       public List<PathNode> pathNodeSearched { get; set; } = new List<PathNode>();
       public PathNode currentNode { get; set; } = new PathNode();
       public const int CURRENTNODE_MAXCOUNT = 3;

        public bool ScanLeft(PathNode[,] pathMap)
        {
            bool leftisScanned = false;
            int coordinate_X = currentNode.coordinate_X;
            int coordinate_Y = currentNode.coordinate_Y - 1;

            if (coordinate_Y >= 0)
            {
                if (!avoidNodeList.Contains(pathMap[coordinate_X, coordinate_Y]) && !bottleNeckList.Contains(pathMap[coordinate_X, coordinate_Y]))
                {
                    if (currentNode.data > pathMap[coordinate_X, coordinate_Y].data)
                    {
                        currentNode = pathMap[coordinate_X, coordinate_Y];
                        pathNodeSearched.Add(pathMap[coordinate_X, coordinate_Y]);
                        leftisScanned = true;
                    }
                }
            }

            return leftisScanned;
        }

        public bool ScanRight(PathNode[,] pathMap,int mapMaxCountY)
        {
            bool rightisScanned = false;

            int coordinate_X = currentNode.coordinate_X;
            int coordinate_Y = currentNode.coordinate_Y + 1;


            if (coordinate_Y <= mapMaxCountY - 1)
            {
                if (!avoidNodeList.Contains(pathMap[coordinate_X, coordinate_Y]) && !bottleNeckList.Contains(pathMap[coordinate_X, coordinate_Y]))
                {
                    if (currentNode.data > pathMap[coordinate_X, coordinate_Y].data)
                    {
                        currentNode = pathMap[coordinate_X, coordinate_Y];
                        pathNodeSearched.Add(pathMap[coordinate_X, coordinate_Y]);
                        rightisScanned = true;
                    }
                }
            }

            return rightisScanned;
        }

        public bool ScanDownward(PathNode[,] pathMap,int mapMaxCountX)
        {
            bool downwardisScanned = false;
            int coordinate_X = currentNode.coordinate_X+1;
            int coordinate_Y = currentNode.coordinate_Y;

            if (coordinate_X <= mapMaxCountX - 1)
            {
                if (!avoidNodeList.Contains(pathMap[coordinate_X, coordinate_Y]) && !bottleNeckList.Contains(pathMap[coordinate_X, coordinate_Y]))
                {
                    if (currentNode.data > pathMap[coordinate_X, coordinate_Y].data)
                    {
                        currentNode = pathMap[coordinate_X, coordinate_Y];
                        pathNodeSearched.Add(pathMap[coordinate_X, coordinate_Y]);
                        downwardisScanned = true;
                    }
                }
            }

            if (!downwardisScanned)
            {
                if (pathNodeSearched.Count > 1)
                {
                    this.avoidNodeList.Add(pathNodeSearched[pathNodeSearched.Count - 1]);
                    this.pathNodeSearched.RemoveAt(pathNodeSearched.Count - 1);
                }

                if (pathNodeSearched.Count > 0)
                    currentNode = pathNodeSearched[pathNodeSearched.Count - 1];

            }

            return downwardisScanned;
        }

    }
}
