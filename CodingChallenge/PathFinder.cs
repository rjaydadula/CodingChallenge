using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingChallenge
{
    public class PathNode
    {
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
                            if (pathMap[mapIndex_X, mapIndex_Y].data == highestNodeValue.data && highestNodeValue.data > 0 || pathMap[mapIndex_X, mapIndex_Y].data >= map.MaxCountX-mapIndex_X && highestNodeValue.data > 0)
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

        private void GetAllBottleNeckNodes(PathNode[,] pathMap, ChallengeMap map, HashSet<PathNode> bottleNeckNodes)
        {
            for (int mapIndex_X = 0; mapIndex_X < map.MaxCountX; mapIndex_X++)
            {
                for (int mapIndex_Y = 0; mapIndex_Y < map.MaxCountY; mapIndex_Y++)
                {
                    if (pathMap[mapIndex_X, mapIndex_Y].data < map.MaxCountX - mapIndex_X)
                        bottleNeckNodes.Add(pathMap[mapIndex_X, mapIndex_Y]);
                }
            }

        }

        private List<List<PathNode>> findPath(PathNode[,] pathMap, ChallengeMap map, List<PathNode> targetNodes)
        {
            DirectionScanner scanner = new DirectionScanner();

            int nodeCounter = 0;

            //GET ALL BOTTLENECK NODES 
            GetAllBottleNeckNodes(pathMap, map, scanner.bottleNeckList);


            List<PathNode> NodeCleared = new List<PathNode>();

            //Scan Twice to properly scan the whole Map
            for (int scanning = 0; scanning < 2; scanning++)
            {
                foreach (PathNode node in targetNodes)
                {
                    nodeCounter++;
                    string valuesCount = (((double)nodeCounter / (targetNodes.Count()*2)) * 100).ToString();
                    Console.WriteLine(" Scanning ChallengeMap: " + (int)Convert.ToDouble(valuesCount, CultureInfo.InvariantCulture.NumberFormat) + "%");
                    Console.SetCursorPosition(0, 0);

                    scanner.pathNodeSearched = new List<PathNode>();
                    scanner.avoidNodeList = new List<PathNode>();

                    scanner.pathNodeSearched.Add(pathMap[node.coordinate_X, node.coordinate_Y]);
                    scanner.currentNode = pathMap[node.coordinate_X, node.coordinate_Y];

                    bool hasScanned = false;
                    int maxcounter = 0;

                    while (true)
                    {
                        if (scanner.bottleNeckList.Contains(scanner.currentNode))
                        {
                            scanner.pathNodeSearched.Remove(scanner.currentNode);
                            scanner.currentNode = scanner.pathNodeSearched[scanner.pathNodeSearched.Count - 1];
                        }


                    //Scanning Left Direction - FirstScan
                    if (scanning == 0)
                    {
                          hasScanned = scanner.ScanLeft(pathMap);

                            //Left Scan
                            if (hasScanned)
                            {
                                NodeCleared = ClearAvoidNodeListBaseOnDirection(map, scanner.currentNode, scanner.avoidNodeList, Direction.Left);

                                if (NodeCleared.Count == 0)
                                    NodeCleared = ClearAvoidNodeListBaseOnDirection(map, scanner.currentNode, scanner.avoidNodeList, Direction.Down);

                                continue;
                            }
                            else 
                                hasScanned = scanner.ScanRight(pathMap, map.MaxCountY);

                            //Right Scan
                            if (hasScanned)
                            {
                                NodeCleared = ClearAvoidNodeListBaseOnDirection(map, scanner.currentNode, scanner.avoidNodeList, Direction.Right);

                                if (NodeCleared.Count == 0)
                                    NodeCleared = ClearAvoidNodeListBaseOnDirection(map, scanner.currentNode, scanner.avoidNodeList, Direction.Down);

                                continue;
                            }
                    }
                    else //Scanning Right Direction - SecondScan
                    {
                        hasScanned = scanner.ScanRight(pathMap,map.MaxCountY);

                            //Right Scan
                            if (hasScanned)
                            {
                                NodeCleared = ClearAvoidNodeListBaseOnDirection(map, scanner.currentNode, scanner.avoidNodeList, Direction.Right);

                                if (NodeCleared.Count == 0)
                                    NodeCleared = ClearAvoidNodeListBaseOnDirection(map, scanner.currentNode, scanner.avoidNodeList, Direction.Down);

                                continue;
                            }
                            else
                                hasScanned = scanner.ScanLeft(pathMap);

                            //Left Scan
                            if (hasScanned)
                            {
                                NodeCleared = ClearAvoidNodeListBaseOnDirection(map, scanner.currentNode, scanner.avoidNodeList, Direction.Left);

                                if (NodeCleared.Count == 0)
                                    NodeCleared = ClearAvoidNodeListBaseOnDirection(map, scanner.currentNode, scanner.avoidNodeList, Direction.Down);

                                continue;
                            }
                    }



                        if (!hasScanned)
                        {
                            hasScanned = scanner.ScanDownward(pathMap, map.MaxCountX);


                            if (NodeCleared.Count() > 0)
                            {
                                //Add All Cleared Nodes in PreviousStage
                                AddAvoidList(map, new HashSet<PathNode>() { scanner.currentNode }, scanner.avoidNodeList, true);
                                NodeCleared = new List<PathNode>();
                            }

                            if (hasScanned)
                                NodeCleared = ClearAvoidNodeListBaseOnDirection(map, scanner.currentNode, scanner.avoidNodeList, Direction.Down);


                            if (hasScanned)
                            {
                                //CHECK IF END OF MAP IS REACHED
                                if (scanner.pathNodeSearched[scanner.pathNodeSearched.Count - 1].coordinate_X == map.MaxCountX - 1)
                                {
                                    if (isPathSearchedAlreadyAdded(scanner.pathNodeInTotalSearched, scanner.pathNodeSearched))
                                        break;
                                    else
                                        scanner.pathNodeInTotalSearched.Add(scanner.pathNodeSearched);


                                    AddAvoidList(map, scanner.pathNodeSearched, scanner.avoidNodeList);


                                    scanner.pathNodeSearched = new List<PathNode>();
                                    scanner.currentNode = pathMap[node.coordinate_X, node.coordinate_Y];

                                    ClearAllAvoidNodeInCurrentXIndex(scanner.currentNode.coordinate_X, map.MaxCountY, scanner.avoidNodeList);

                                    scanner.pathNodeSearched.Add(pathMap[node.coordinate_X, node.coordinate_Y]);
                                }



                                continue;
                            }

                            if (!hasScanned && scanner.currentNode == scanner.pathNodeSearched[0])
                            {
                                maxcounter++;
                                if (maxcounter >= DirectionScanner.CURRENTNODE_MAXCOUNT)
                                    break;
                            }

                        }

                    }

                }
            }

            return scanner.pathNodeInTotalSearched;
        }

        private void Scanning()
        {

        }

        private bool isPathSearchedAlreadyAdded(List<List<PathNode>> pathNodeInTotalSearched, List<PathNode> pathNodeList)
        {
            foreach (var listPathNode in pathNodeInTotalSearched)
            {
                if (listPathNode.Count == pathNodeList.Count)
                {
                    int index = 0;
                    bool ismatched = true;
                    foreach (var PathNode in listPathNode)
                    {
                        if (PathNode != pathNodeList[index])
                        {
                            ismatched = false;
                            break;
                        }

                        index++;
                    }

                    if (ismatched)
                        return true;
                }
            }

            return false;
        }

        private void AddAvoidList(ChallengeMap map,List<PathNode> pathNodeSearchedList, List<PathNode> avoidNodeList,bool includeTopNode=false)
        {
           
                int index = 0;
                foreach (PathNode node in pathNodeSearchedList)
                {
                    if (includeTopNode)
                    {
                        if (!avoidNodeList.Contains(node) && node.coordinate_X != map.MaxCountX - 1)
                            avoidNodeList.Add(node);
                    }
                    else
                    {
                        if (index > 0)
                        {
                            if (!avoidNodeList.Contains(node) && node.coordinate_X != map.MaxCountX - 1)
                                avoidNodeList.Add(node);
                        }
                    }

                    index++;
                }
                
                //return avoidNodeList;
        }

        private void AddAvoidList(ChallengeMap map, HashSet<PathNode> pathNodeSearchedList, List<PathNode> avoidNodeList, bool includeTopNode = false)
        {

            int index = 0;
            foreach (PathNode node in pathNodeSearchedList)
            {
                if (includeTopNode)
                {
                    if (!avoidNodeList.Contains(node) && node.coordinate_X != map.MaxCountX-1)
                        avoidNodeList.Add(node);
                }
                else
                {
                    if (index > 0)
                    {
                        if (!avoidNodeList.Contains(node) && node.coordinate_X != map.MaxCountX - 1)
                            avoidNodeList.Add(node);
                    }
                }

                index++;
            }

            //return avoidNodeList;
        }

        private void ClearAllAvoidNodeInCurrentXIndex(int index_X,int mapYMaxCount, List<PathNode> avoidNodeList)
        {
            List<PathNode> indexRemoveList = new List<PathNode>();

            for(int index=0; index < avoidNodeList.Count;index++)
            {
                if(avoidNodeList[index].coordinate_X == index_X && avoidNodeList[index].coordinate_Y > 0 && avoidNodeList[index].coordinate_Y  < mapYMaxCount - 1)
                    avoidNodeList.Remove(avoidNodeList[index]);
            }
        }

        private List<PathNode> ClearAvoidNodeListBaseOnDirection(ChallengeMap map,PathNode currentNode, List<PathNode> avoidNodeList,int direction)
        {
            List<PathNode> indexRemoveList = new List<PathNode>();

            int coordinate_X = currentNode.coordinate_X;

            if(direction == Direction.Down)
            {
                coordinate_X = currentNode.coordinate_X + 1;
            }

            for (int index = 0; index < avoidNodeList.Count; index++)
            {
                if (avoidNodeList[index].coordinate_X == coordinate_X)
                {
                    //CHECK CURRENT SCAN DIRECTION
                    switch (direction)
                    {
                        case Direction.Left:
                            {
                                if (currentNode.coordinate_Y > 0)
                                {
                                    if (avoidNodeList[index].coordinate_Y < currentNode.coordinate_Y)
                                    {
                                        indexRemoveList.Add(avoidNodeList[index]);
                                        avoidNodeList.Remove(avoidNodeList[index]);
                                        break;
                                    }
                                }
                                break;
                            }
                        case Direction.Right:
                            {

                                if (currentNode.coordinate_Y < map.MaxCountY - 1)
                                {
                                    if (avoidNodeList[index].coordinate_Y > currentNode.coordinate_Y)
                                    {
                                        indexRemoveList.Add(avoidNodeList[index]);
                                        avoidNodeList.Remove(avoidNodeList[index]);
                                        break;
                                    }
                                }
                                break;
                            }
                        case Direction.Down:
                            {
                                if (avoidNodeList[index].coordinate_Y != currentNode.coordinate_Y && currentNode.coordinate_Y > 0)
                                {
                                    if (avoidNodeList[index].coordinate_Y == currentNode.coordinate_Y - 1)
                                    {
                                        indexRemoveList.Add(avoidNodeList[index]);
                                        avoidNodeList.Remove(avoidNodeList[index]);
                                    }
                                }
                                else if(avoidNodeList[index].coordinate_Y != currentNode.coordinate_Y && currentNode.coordinate_Y < map.MaxCountY-1)
                                {
                                    if (avoidNodeList[index].coordinate_Y == currentNode.coordinate_Y + 1)
                                    {
                                        indexRemoveList.Add(avoidNodeList[index]);
                                        avoidNodeList.Remove(avoidNodeList[index]);
                                    }
                                }


                                break;
                            }

                    }
                }
                    

                
            }

            return indexRemoveList;
        }

    }
}
