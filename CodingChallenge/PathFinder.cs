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

    public class Node
    {
        public string east { get; set; }
        public string west { get; set; }
        public string south { get; set; }
        public string center { get; set; }
        public string coordinate { get; set; }
        public int box_Count { get; set; }
    }

    public class PathFinder
    {
        public Dictionary<int, List<Node>> nodeListPerIndex { get; set; }
        public int LengthOfCalculatedPath { get; set; }
        public int DropOfCalculatedPath { get; set; }
        public string CalculatedPath { get; set; }

        public void CreatePath(ChallengeMap map)
        {
            nodeListPerIndex = new Dictionary<int, List<Node>>();
            int box_counter = 0;
            //Populating Node with Data, Based on the Map
            for (int mapIndex_X = 0; mapIndex_X < map.MaxCountX; mapIndex_X++)
            {
                box_counter = mapIndex_X;
                for (int mapIndex_Y = 0; mapIndex_Y < map.MaxCountY; mapIndex_Y++)
                {
                    Node node = new Node();

                    node.center = map.challengeMap[mapIndex_X, mapIndex_Y];
                    node.coordinate = mapIndex_X + "," + mapIndex_Y;
                    node.box_Count = box_counter;

                    if (mapIndex_Y == 0)
                        node.east = null;
                    else
                        node.east = map.challengeMap[mapIndex_X, mapIndex_Y - 1];


                    if (mapIndex_Y == map.MaxCountY - 1)
                        node.west = null;
                    else
                        node.west = map.challengeMap[mapIndex_X, mapIndex_Y + 1];


                    if (mapIndex_X == map.MaxCountX - 1)
                        node.south = null;
                    else
                        node.south = map.challengeMap[mapIndex_X + 1, mapIndex_Y];


                    if (!nodeListPerIndex.ContainsKey(mapIndex_X))
                        nodeListPerIndex.Add(mapIndex_X, new List<Node>() { node });
                    else
                        nodeListPerIndex[mapIndex_X].Add(node);


                    box_counter++;
                }
            }

            int[,] mapPath = new int[map.MaxCountX,map.MaxCountY];
            PathNode[,] pathMap = new PathNode[map.MaxCountX, map.MaxCountY];

            for (int mapIndex_X = 0; mapIndex_X < map.MaxCountX; mapIndex_X++)
            {
                box_counter = mapIndex_X;
                for (int mapIndex_Y = 0; mapIndex_Y < map.MaxCountY; mapIndex_Y++)
                {
                    PathNode node = new PathNode();
                    node.box_Count = box_counter;
                    node.data = int.Parse(map.challengeMap[mapIndex_X, mapIndex_Y]);
                    node.coordinate_X = mapIndex_X;
                    node.coordinate_Y = mapIndex_Y;

                    pathMap[mapIndex_X, mapIndex_Y] = node;

                    box_counter++;
                }
            }

            List<PathNode> targetNodes = new List<PathNode>();

            targetNodes = findRooTNodeCoordinate(DropOfCalculatedPath,map,pathMap);

            List<List<PathNode>> TotalPath_List = findPath(pathMap, map, targetNodes);

        }

        private List<PathNode> findRooTNodeCoordinate(int DropOfCalculatedPath, ChallengeMap map, PathNode[,] pathMap)
        {
            //x,y,data
            List<PathNode> nodeList = new List<PathNode>();

            for (int mapIndex_X = 0; mapIndex_X < map.MaxCountX; mapIndex_X++)
            {
                for (int mapIndex_Y = 0; mapIndex_Y < map.MaxCountY; mapIndex_Y++)
                {
                    if(pathMap[mapIndex_X,mapIndex_Y].data == DropOfCalculatedPath)
                    {
                        nodeList.Add(pathMap[mapIndex_X, mapIndex_Y]);
                    }

                }
            }

            return nodeList;
        }

        private List<List<PathNode>> findPath(PathNode[,] pathMap, ChallengeMap map, List<PathNode> targetNodes)
        {
            List<List<PathNode>> pathNodeInTotalSearched = new List<List<PathNode>>();

            PathNode currentNode = new PathNode();
            List<PathNode> avoidNodeList = new List<PathNode>();

            foreach (PathNode node in targetNodes)
            {
                List<PathNode> pathNodeSearched = new List<PathNode>();
                currentNode = pathMap[node.coordinate_X, node.coordinate_Y];
                pathNodeSearched.Add(pathMap[node.coordinate_X, node.coordinate_Y]);

                int coordinate_X = 0;
                int coordinate_Y = 0;
                int isStillFinding = 0;

                int MaxRootReturn = 3;

                while (true)
                {

                    coordinate_X = currentNode.coordinate_X;
                    coordinate_Y = currentNode.coordinate_Y-1;

                    if (coordinate_Y >= 0)
                    {
                        if (!avoidNodeList.Contains(pathMap[coordinate_X, coordinate_Y]))
                        {
                            if (currentNode.data > pathMap[coordinate_X, coordinate_Y].data)
                            {
                                currentNode = pathMap[coordinate_X, coordinate_Y];
                                pathNodeSearched.Add(pathMap[coordinate_X, coordinate_Y]);
                                isStillFinding++;
                            }
                        }
                    }

                    coordinate_X = currentNode.coordinate_X;
                    coordinate_Y = currentNode.coordinate_Y+1;

                    if (coordinate_Y <= map.MaxCountY - 1)
                    {
                        if (!avoidNodeList.Contains(pathMap[coordinate_X, coordinate_Y]))
                        {
                            if (currentNode.data > pathMap[coordinate_X, coordinate_Y].data)
                            {
                                currentNode = pathMap[coordinate_X, coordinate_Y];
                                pathNodeSearched.Add(pathMap[coordinate_X, coordinate_Y]);
                                isStillFinding++;
                            }
                        }
                    }

                    coordinate_X = currentNode.coordinate_X + 1;
                    coordinate_Y = currentNode.coordinate_Y;

                    if (isStillFinding == 0 && coordinate_X <= map.MaxCountX-1 && currentNode.data <= pathMap[coordinate_X, coordinate_Y].data)
                    {
                        avoidNodeList.Add(pathNodeSearched[pathNodeSearched.Count - 1]);
                        pathNodeSearched.RemoveAt(pathNodeSearched.Count - 1);
                        currentNode = pathNodeSearched[pathNodeSearched.Count - 1];
                        continue;
                    }
                    else
                    {
                        if(avoidNodeList.Count() > 0 && isStillFinding == 0)
                        {
                            if(pathNodeSearched.Count - 1 <= 0)
                            {
                                MaxRootReturn = MaxRootReturn - 1;

                                //Check IF Maximum Return to The Original Root Location Reached
                                if(MaxRootReturn == 0)
                                {
                                    if (coordinate_X <= map.MaxCountX - 1)
                                    {
                                        if (currentNode.data > pathMap[coordinate_X, coordinate_Y].data)
                                        {
                                            currentNode = pathMap[coordinate_X, coordinate_Y];
                                            pathNodeSearched.Add(pathMap[coordinate_X, coordinate_Y]);

                                            //Remove AvoidList In Current Index Scanned
                                            avoidNodeList = ClearAllAvoidNodeInCurrentXIndex(coordinate_X, avoidNodeList);

                                            if (coordinate_X == map.MaxCountX - 1)
                                            {
                                                pathNodeInTotalSearched.Add(pathNodeSearched);
                                                pathNodeSearched = new List<PathNode>();

                                                avoidNodeList = AddAvoidList(pathNodeInTotalSearched, avoidNodeList);

                                                if (MaxRootReturn != 0)
                                                {
                                                    continue;
                                                }

                                                break;
                                            }
                                            isStillFinding++;
                                        }
                                    }
                                }

                                continue;
                            }

                            avoidNodeList.Add(pathNodeSearched[pathNodeSearched.Count - 1]);
                            pathNodeSearched.RemoveAt(pathNodeSearched.Count - 1);
                            currentNode = pathNodeSearched[pathNodeSearched.Count - 1];
                            continue;
                        }
                    }

                    

                    if (coordinate_X <= map.MaxCountX - 1)
                    {
                        if (currentNode.data > pathMap[coordinate_X, coordinate_Y].data)
                        {
                            currentNode = pathMap[coordinate_X, coordinate_Y];
                            pathNodeSearched.Add(pathMap[coordinate_X, coordinate_Y]);

                            //Remove AvoidList In Current Index Scanned
                            avoidNodeList = ClearAllAvoidNodeInCurrentXIndex(coordinate_X, avoidNodeList);

                            if (coordinate_X == map.MaxCountX - 1)
                            {
                                pathNodeInTotalSearched.Add(pathNodeSearched);
                                pathNodeSearched = new List<PathNode>();

                                avoidNodeList = AddAvoidList(pathNodeInTotalSearched,avoidNodeList);

                                if (MaxRootReturn != 0)
                                {
                                    continue;
                                }

                                break;
                            }
                            isStillFinding++;
                        }
                    }

                    if (isStillFinding == 0)
                    {
                        pathNodeSearched = new List<PathNode>();
                    }
                    else
                    {
                        isStillFinding = 0;
                    }
                }
                
            }

            return pathNodeInTotalSearched;
        }

        private List<PathNode> AddAvoidList(List<List<PathNode>> pathNodeSearchedList, List<PathNode> avoidNodeList)
        {
            
            foreach(List<PathNode> upperList in pathNodeSearchedList)
            {
                foreach(PathNode node in upperList)
                {
                    if (!avoidNodeList.Contains(node))
                        avoidNodeList.Add(node);
                }
            }
                return avoidNodeList;
        }

        private List<PathNode> ClearAllAvoidNodeInCurrentXIndex(int index_X, List<PathNode> avoidNodeList)
        {
            List<PathNode> indexRemoveList = new List<PathNode>();

            for(int index=0; index < avoidNodeList.Count;index++)
            {
                if (indexRemoveList.Contains(avoidNodeList[index]))
                    avoidNodeList.Remove(avoidNodeList[index]);
            }

            return avoidNodeList;
        }

    }
}
