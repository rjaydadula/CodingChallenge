using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingChallenge
{
    class Program
    {
        

        static void Main(string[] args)
        {

            ChallengeMap map = new ChallengeMap(4,4);
            PathFinder pathFinder = new PathFinder();
            pathFinder.DropOfCalculatedPath = 4;
            map.Create();


            pathFinder.CreatePath(map);

            //NodeTree nodeRoot = new NodeTree(pathFinder.DropOfCalculatedPath, 0);

            //NodeTree nodeTree = new NodeTree();

            //nodeTree.AddEastNode(nodeRoot, nodeRoot);



            //NodeTree currentEastNodeTree = new NodeTree();
            //NodeTree currentWestNodeTree = new NodeTree();
            //NodeTree currentSouthNodeTree = new NodeTree();


            //currentSouthNodeTree = nodeTree;


            //NodeTree TreeEastNode = new NodeTree();
            //TreeEastNode.data = "1";



            //nodeTree.AddEastNode(currentEastNodeTree, TreeEastNode);
            //currentEastNodeTree = TreeEastNode;

            //NodeTree TreeEastNode1 = new NodeTree();
            //TreeEastNode1.data = "4";

            //nodeTree.AddEastNode(currentEastNodeTree, TreeEastNode1);
            //currentEastNodeTree = TreeEastNode1;

             
            ////ADD West Node
            //NodeTree TreeWestNode = new NodeTree();
            //TreeWestNode.data = "12";

            //nodeTree.AddWestNode(currentWestNodeTree, TreeWestNode);
            //currentWestNodeTree = TreeWestNode;

            //NodeTree TreeWestNode1 = new NodeTree();
            //TreeWestNode1.data = "23";

            //nodeTree.AddWestNode(currentWestNodeTree, TreeWestNode1);
            //currentWestNodeTree = TreeWestNode1;

            //NodeTree TreeSouthNode = new NodeTree();
            //TreeSouthNode.data = "9";

            //nodeTree.AddSouthNode(currentSouthNodeTree, TreeSouthNode);
            //currentSouthNodeTree = TreeSouthNode;

            Console.Write("\n\n");
            Console.WriteLine("Length Of Calculated Path: "+ pathFinder.LengthOfCalculatedPath);
            Console.WriteLine("Drop Of Calculated Path: " + pathFinder.DropOfCalculatedPath);
            Console.WriteLine("Calculated Path: " + pathFinder.CalculatedPath);

            Console.Read();

        }
    }
}
