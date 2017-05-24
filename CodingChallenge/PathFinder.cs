using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingChallenge
{
    public class PathNode
    {
        public int box_Count { get; set; } = new int();
        public int data { get; set; } = new int();
        public int coordinate_X { get; set; }
        public int coordinate_Y { get; set; }
    }

    public class PathFinder
    {
        public int LengthOfCalculatedPath { get; set; }
        public int DropOfCalculatedPath { get; set; }
        public string CalculatedPath { get; set; }

        public void SearchPath(ChallengeMap map)
        {
            int[,] mapPath = new int[map.MaxCountX,map.MaxCountY];
            PathNode[,] pathMap = new PathNode[map.MaxCountX, map.MaxCountY];

            for (int mapIndex_X = 0; mapIndex_X < map.MaxCountX; mapIndex_X++)
            {
                for (int mapIndex_Y = 0; mapIndex_Y < map.MaxCountY; mapIndex_Y++)
                {
                    PathNode node = new PathNode();
                    node.data = int.Parse(map.challengeMap[mapIndex_X, mapIndex_Y]);
                    node.coordinate_X = mapIndex_X;
                    node.coordinate_Y = mapIndex_Y;

                    pathMap[mapIndex_X, mapIndex_Y] = node;
                }
            }

            List<PathNode> targetNodes = new List<PathNode>();

            targetNodes = findAllStartingNode(map,pathMap);

            List<List<PathNode>> TotalPath_List = findPath(pathMap, map, targetNodes).OrderByDescending(x=>x.Count).ToList();

            if (TotalPath_List.Count > 0)
            {
                TotalPath_List = filterTopPaths(TotalPath_List);
                findSteepestNodePath(TotalPath_List);
            }

        }

        private void findSteepestNodePath(List<List<PathNode>> TotalPath_List)
        {
            List<PathNode> steepestNodes = new List<PathNode>();
            PathNode node = new PathNode();
            node.data = 1;
            steepestNodes.Add(node);

            PathNode node1 = new PathNode();
            node1.data = 1;
            steepestNodes.Add(node1);


            foreach (List<PathNode> nodelist in TotalPath_List)
            {
                int steepestCount = nodelist[0].data - nodelist[nodelist.Count - 1].data;
                int mainCollectionCount = steepestNodes[0].data - steepestNodes[steepestNodes.Count - 1].data;

                if (steepestCount > mainCollectionCount)
                    steepestNodes = nodelist;
            }

            int index = 0;
            foreach(PathNode pathnode in steepestNodes)
            {
                if (index < steepestNodes.Count - 1)
                    CalculatedPath += string.Concat(pathnode.data + "-");
                else
                    CalculatedPath += pathnode.data;

                index++;
            }

            LengthOfCalculatedPath = steepestNodes.Count;
            DropOfCalculatedPath = steepestNodes[0].data - steepestNodes[steepestNodes.Count - 1].data;
        }

        private List<List<PathNode>> filterTopPaths(List<List<PathNode>> TotalPath_List)
        {
            int highestPathCount = TotalPath_List[0].Count;

            for(int index=1;index < TotalPath_List.Count;index++)
            {
                if (TotalPath_List[index].Count != highestPathCount)
                {
                    TotalPath_List.RemoveAt(index);
                    index--;
                }
            }

            return TotalPath_List;
        }

        private List<PathNode> findAllStartingNode(ChallengeMap map, PathNode[,] pathMap)
        {
            //x,y,data
            List<PathNode> nodeList = new List<PathNode>();

            List<PathNode> nodeListSameValue = new List<PathNode>();

            for (int mapIndex_X = 0; mapIndex_X < map.MaxCountX; mapIndex_X++)
            {
                PathNode highestNodeValue = new PathNode();

                for (int mapIndex_Y = 0; mapIndex_Y < map.MaxCountY; mapIndex_Y++)
                {
                    if (mapIndex_X < map.MaxCountX - 1)
                    {
                        if (pathMap[mapIndex_X, mapIndex_Y].data > highestNodeValue.data)
                        {

                            highestNodeValue = pathMap[mapIndex_X, mapIndex_Y];

                            nodeListSameValue = new List<PathNode>();
                            nodeListSameValue.Add(highestNodeValue);

                        }
                        else
                        {
                            if (pathMap[mapIndex_X, mapIndex_Y].data == highestNodeValue.data && highestNodeValue.data > 0)
                            {
                                nodeListSameValue.Add(pathMap[mapIndex_X, mapIndex_Y]);
                            }
                        }
                    }
                }

                if (highestNodeValue.data > 0 && nodeListSameValue.Count > 0)
                {
                    foreach (PathNode node in nodeListSameValue)
                    {
                        nodeList.Add(node);
                    }

                    nodeListSameValue = new List<PathNode>();
                }
            }

            return nodeList;
        }

        private List<List<PathNode>> findPath(PathNode[,] pathMap, ChallengeMap map, List<PathNode> targetNodes)
        {
            DirectionScanner scanner = new DirectionScanner();

            List<List<PathNode>> pathNodeInTotalSearched = new List<List<PathNode>>();

            foreach (PathNode node in targetNodes)
            {
                scanner.pathNodeSearched = new List<PathNode>();
                scanner.avoidNodeList = new List<PathNode>();

                scanner.pathNodeSearched.Add(pathMap[node.coordinate_X, node.coordinate_Y]);
                scanner.currentNode = pathMap[node.coordinate_X, node.coordinate_Y];
                bool hasScanned = false;
                int maxcounter = 0;

                while (true)
                {
                    hasScanned = scanner.ScanLeft(pathMap);

                    //Left Scan
                    if (hasScanned)
                        continue;
                    else
                        hasScanned = scanner.ScanRight(pathMap, map.MaxCountY);

                    //Right Scan
                    if (hasScanned)
                        continue;

                    if (!hasScanned)
                    {
                        hasScanned = scanner.ScanDownward(pathMap, map.MaxCountX);

                        if (hasScanned)
                        {
                            //CHECK IF END OF MAP IS REACHED
                            if (scanner.pathNodeSearched[scanner.pathNodeSearched.Count - 1].coordinate_X == map.MaxCountX - 1)
                            {
                                pathNodeInTotalSearched.Add(scanner.pathNodeSearched);
                                scanner.avoidNodeList = AddAvoidList(pathNodeInTotalSearched, scanner.avoidNodeList);
                                

                                scanner.pathNodeSearched = new List<PathNode>();
                                scanner.currentNode = pathMap[node.coordinate_X, node.coordinate_Y];
                                scanner.pathNodeSearched.Add(pathMap[node.coordinate_X, node.coordinate_Y]);
                                //break;
                            }

                            continue;
                        }

                        if (!hasScanned && scanner.currentNode == scanner.pathNodeSearched[0])
                        {
                            maxcounter++;
                            if(maxcounter == DirectionScanner.CURRENTNODE_MAXCOUNT)
                            break;
                        }

                    }

                }

            }

            return pathNodeInTotalSearched;
        }

        private List<PathNode> AddAvoidList(List<List<PathNode>> pathNodeSearchedList, List<PathNode> avoidNodeList)
        {
            
            foreach(List<PathNode> upperList in pathNodeSearchedList)
            {
                int index = 0;
                foreach (PathNode node in upperList)
                {
                    if (index > 0)
                    {
                        if (!avoidNodeList.Contains(node))
                            avoidNodeList.Add(node);
                    }

                    index++;
                }
                
            }
                return avoidNodeList;
        }

        private List<PathNode> ClearAllAvoidNodeInCurrentXIndex(int index_X, List<PathNode> avoidNodeList)
        {
            List<PathNode> indexRemoveList = new List<PathNode>();

            for(int index=0; index < avoidNodeList.Count;index++)
            {
                if(avoidNodeList[index].coordinate_X == index_X)
                    avoidNodeList.Remove(avoidNodeList[index]);
            }

            return avoidNodeList;
        }

    }
}
