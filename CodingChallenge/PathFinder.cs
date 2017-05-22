using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingChallenge
{
    public class Node
    {
        public string east { get; set; }
        public string west { get; set; }
        public string south { get; set; }
        public string center { get; set; }
        public string coordinate { get; set; }
    }

    public class TreeNode
    {
        public int stageCount { get; set; }
        public TreeNode east { get; set; }
        public TreeNode west { get; set; }
        public TreeNode north { get; set; }
        public TreeNode south { get; set; }
        public string data { get; set; }

        public TreeNode()
        {
            this.east = null;
            this.west = null;
            this.north = null;
            this.south = null;
            this.data = null;
            this.stageCount = 0;
        }

        public TreeNode(int data, int stageCount)
        {
            this.east = null;
            this.west = null;
            this.north = null;
            this.south = null;
            this.stageCount = stageCount;

            this.data = data.ToString();
        }

        public void AddEastNode(TreeNode currentNode, TreeNode newNode)
        {
            //Add Root Node Data If no root Node has Been Added
            if (this.data == null && this.north == null && this.south == null && this.east == null && this.west == null)
            {
                this.data = newNode.data;
                this.stageCount = newNode.stageCount;
                return;
            }

            TreeNode mainNode = new TreeNode();

            if (currentNode.data == null)
            {
                mainNode = this;
                newNode.west = this;
                newNode.stageCount = newNode.west.stageCount + 1;
                mainNode.east = newNode;
            }
            else
            {
                mainNode = this;
                newNode.west = currentNode;
                newNode.stageCount = newNode.west.stageCount + 1;
                currentNode.east = newNode;
                //mainNode.east = currentNode;
            }

        }

        public void AddWestNode(TreeNode currentNode, TreeNode newNode)
        {
            //Add Root Node Data If no root Node has Been Added
            if (this.data == null && this.north == null && this.south == null && this.east == null && this.west == null)
            {
                this.data = newNode.data;
                this.stageCount = newNode.stageCount;
                return;
            }

            TreeNode mainNode = new TreeNode();

            if (currentNode.data == null)
            {
                mainNode = this;
                newNode.east = this;
                newNode.stageCount = newNode.east.stageCount + 1;
                mainNode.west = newNode;
            }
            else
            {
                mainNode = this;
                newNode.east = currentNode;
                newNode.stageCount = newNode.east.stageCount + 1;
                currentNode.west = newNode;
                // mainNode.west = currentNode;
            }
        }

        public void AddSouthNode(TreeNode currentNode, TreeNode newNode)
        {
            //Add Root Node Data If no root Node has Been Added
            if (this.data == null && this.north == null && this.south == null && this.east == null && this.west == null)
            {
                this.data = newNode.data;
                this.stageCount = newNode.stageCount;
                return;
            }

            TreeNode mainNode = new TreeNode();

            if (currentNode.data != null)
            {
                mainNode = this;
                newNode.north = currentNode;
                newNode.stageCount = currentNode.stageCount + 1;
                currentNode.south = newNode;
                if (mainNode.south == null)
                    mainNode.south = newNode;
            }
        }

    }

    public class PathFinder
    {
        public Dictionary<int, List<Node>> nodeListPerIndex { get; set; }
        public int LengthOfCalculatedPath { get; set; }
        public int DropOfCalculatedPath { get; set; }
        public string CalculatedPath { get; set; }

        private void scanLeft(Dictionary<int, Node> lineData, TreeNode mainTree, TreeNode currentEastNode, TreeNode currentSouthNode, int scanIndex)
        {
            int direction = lineData.Count - scanIndex;
            for (int leftScanData = 0; leftScanData <= lineData.Count - scanIndex; leftScanData++)
            {
                TreeNode eastNode = new TreeNode();

                eastNode.data = lineData[direction].center;

                mainTree.AddEastNode(currentEastNode, eastNode);
                currentEastNode = eastNode;
                currentSouthNode = eastNode;

                TreeNode southNode = new TreeNode();
                southNode.data = lineData[direction].south;

                if (southNode.data != null)
                    mainTree.AddSouthNode(currentSouthNode, southNode);

                direction--;
            }

        }

        private void scanRight(List<Node> node, int index, TreeNode mainTree, TreeNode currentWestNode, TreeNode currentSouthNode)
        {
            //if (index < node.Count-1)
            //{
            for (int rightScanData = index; rightScanData < node.Count; rightScanData++)
            {
                //Check If Last West Index
                //if (rightScanData < node.Count - 1)
                //{
                TreeNode westNode = new TreeNode();

                westNode.data = node[rightScanData].center;

                mainTree.AddWestNode(currentWestNode, westNode);
                currentWestNode = westNode;
                currentSouthNode = westNode;

                TreeNode southNode = new TreeNode();
                southNode.data = node[rightScanData].south;

                if (southNode.data != null)
                    mainTree.AddSouthNode(currentSouthNode, southNode);
                //}
            }
            //}
        }

        public TreeNode CreatePath(ChallengeMap map)
        {
            nodeListPerIndex = new Dictionary<int, List<Node>>();

            //Populating Node with Data, Based on the Map
            for (int mapIndex_X = 0; mapIndex_X < map.MaxCountX; mapIndex_X++)
            {
                for (int mapIndex_Y = 0; mapIndex_Y < map.MaxCountY; mapIndex_Y++)
                {
                    Node node = new Node();

                    node.center = map.challengeMap[mapIndex_X, mapIndex_Y];
                    node.coordinate = mapIndex_X + "," + mapIndex_Y;

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

                }
            }

            bool isDropOfCalculatedPathFound = false;
            string mapRootCoordinates = string.Empty;

            TreeNode MainTree = new TreeNode();

            TreeNode currentEastNodeTree = new TreeNode();
            TreeNode currentWestNodeTree = new TreeNode();
            TreeNode currentSouthNodeTree = new TreeNode();

            TreeNode currentSouthRootNodeTree = new TreeNode();

            int Upperindex = 0;
            string rootNextSouth = string.Empty;

            bool isrootFound = false;
            int rootIndex_Y = 0;
            int scanIndex = 0;

            foreach (var dictionaryData in nodeListPerIndex)
            {
                Upperindex++;
                int Lowerindex = 0;
                bool isScanLeft = false;
                Dictionary<int, Node> dataLeftScan = new Dictionary<int, Node>();

                foreach (var data in dictionaryData.Value)
                {

                    if (isDropOfCalculatedPathFound)
                    {
                        if (Upperindex != nodeListPerIndex.Count)
                        {
                            if (!isScanLeft)
                            {
                                scanLeft(dataLeftScan, MainTree, currentEastNodeTree, currentSouthNodeTree, scanIndex);
                                isScanLeft = true;
                            }

                            if (Lowerindex == dictionaryData.Value.Count - 1)
                                scanRight(dictionaryData.Value, rootIndex_Y + 1, MainTree, currentWestNodeTree, currentSouthNodeTree);

                        }

                    }
                    else
                    {
                        //Find Root Value
                        if (data.center == DropOfCalculatedPath.ToString())
                        {
                            isrootFound = true;
                            rootIndex_Y = Lowerindex;
                            scanIndex = 1;
                            isDropOfCalculatedPathFound = true;

                            TreeNode rootNode = new TreeNode(DropOfCalculatedPath, 1);

                            MainTree.AddEastNode(rootNode, rootNode);
                            currentSouthRootNodeTree = rootNode;

                            TreeNode southNode = new TreeNode();
                            southNode.data = data.south;
                            rootNextSouth = data.south;

                            if (southNode.data != null)
                            {
                                MainTree.AddSouthNode(currentSouthRootNodeTree, southNode);
                                currentSouthRootNodeTree = southNode;
                            }

                            //Check if Root Data is in the Last Index_Y
                            if (Lowerindex == dictionaryData.Value.Count - 1)
                            {
                                scanLeft(dataLeftScan, MainTree, currentEastNodeTree, currentSouthRootNodeTree, scanIndex);
                            }
                        }
                        else
                        {

                            dataLeftScan.Add(Lowerindex, data);

                            if (data.center == currentSouthRootNodeTree.data && Lowerindex == rootIndex_Y)
                            {
                                dataLeftScan.Remove(Lowerindex);

                                scanIndex = 2;
                                isDropOfCalculatedPathFound = true;

                                TreeNode eastNode = new TreeNode();
                                eastNode.data = data.east;

                                if (eastNode.data != null)
                                {
                                    MainTree.AddEastNode(currentSouthRootNodeTree, eastNode);
                                    currentEastNodeTree = eastNode;
                                }

                                TreeNode westNode = new TreeNode();
                                westNode.data = data.west;

                                if (westNode.data != null)
                                {
                                    MainTree.AddWestNode(currentSouthRootNodeTree, westNode);
                                    currentWestNodeTree = westNode;
                                }

                                TreeNode southNode = new TreeNode();
                                southNode.data = data.south;
                                rootNextSouth = data.south;

                                if (southNode.data != null)
                                {
                                    MainTree.AddSouthNode(currentSouthRootNodeTree, southNode);
                                    currentSouthRootNodeTree = southNode;
                                }

                                //Check if Root Data is in the Last Index_Y
                                if (Lowerindex == dictionaryData.Value.Count - 1)
                                {
                                    scanLeft(dataLeftScan, MainTree, currentEastNodeTree, currentSouthRootNodeTree, scanIndex);
                                }
                            }
                        }
                    }

                    Lowerindex++;


                }
                //Reset to get the value below the root value
                if (isDropOfCalculatedPathFound)
                    isDropOfCalculatedPathFound = false;


                if (nodeListPerIndex.Count == Upperindex)
                {
                    scanLeft(dataLeftScan, MainTree, currentEastNodeTree, currentSouthNodeTree, scanIndex);
                    scanRight(dictionaryData.Value, rootIndex_Y + 1, MainTree, currentWestNodeTree, currentSouthNodeTree);
                }
            }


            return MainTree;
        }

        public void ShowLongestPath(TreeNode MainTree, int maxLength_X)
        {
            TreeNode current_Treenode = new TreeNode();


        }
    }
}
