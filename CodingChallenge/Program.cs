using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingChallenge
{
    class Program
    {
        private static int mapMaxCountX = 10;
        private static int mapMaxCountY = 10;

        static void Main(string[] args)
        {
            int[,] challengeMap = new int[mapMaxCountX, mapMaxCountY];
            int valuePerBox = 1500;

            PathFinder pathFinder = new PathFinder();

            //Populating the ChallengeMap with data
            for (int mapIndex_X = 0; mapIndex_X < mapMaxCountX; mapIndex_X++)
            {
                for (int mapIndex_Y = 0; mapIndex_Y < mapMaxCountY; mapIndex_Y++)
                {
                    valuePerBox--;

                    if (valuePerBox == 0)
                        valuePerBox = 1499;

                    challengeMap[mapIndex_X, mapIndex_Y] = valuePerBox;
                }
            }

            //DISPLAYING MAP
            for (int mapIndex_X = 0; mapIndex_X < mapMaxCountX; mapIndex_X++)
            {
                for (int mapIndex_Y = 0; mapIndex_Y < mapMaxCountY; mapIndex_Y++)
                {
                    Console.Write(" " + challengeMap[mapIndex_X, mapIndex_Y].ToString() + " ");

                }
                Console.Write("\n");
            }


            pathFinder.nodeListPerIndex = new Dictionary<int, List<Node>>();

            for (int mapIndex_X = 0; mapIndex_X < mapMaxCountX; mapIndex_X++)
            {
                for (int mapIndex_Y = 0; mapIndex_Y < mapMaxCountY; mapIndex_Y++)
                {
                    Node node = new Node();

                    if (mapIndex_Y == 0)
                        node.east = 1500;
                    else
                        node.east = challengeMap[mapIndex_X, mapIndex_Y - 1];


                    if (mapIndex_Y == mapMaxCountY - 1)
                        node.west = 1500;
                    else
                        node.west = challengeMap[mapIndex_X, mapIndex_Y + 1];


                    if (mapIndex_X == mapMaxCountX - 1)
                        node.south = 1500;
                    else
                        node.south = challengeMap[mapIndex_X + 1, mapIndex_Y];


                    if (!pathFinder.nodeListPerIndex.ContainsKey(mapIndex_X))
                        pathFinder.nodeListPerIndex.Add(mapIndex_X, new List<Node>() { node });
                    else
                        pathFinder.nodeListPerIndex[mapIndex_X].Add(node);

                }
            }



            Console.Write("\n\n");
            Console.WriteLine("Length Of Calculated Path: "+ pathFinder.LengthOfCalculatedPath);
            Console.WriteLine("Drop Of Calculated Path: " + pathFinder.DropOfCalculatedPath);
            Console.WriteLine("Calculated Path: " + pathFinder.CalculatedPath);

            Console.Read();

        }
    }
}
