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
            for (int mapIndex = 0; mapIndex < challengeMap.GetLength(0); mapIndex++)
            {
                for (int mapctrPerIndex = 0; mapctrPerIndex < challengeMap.GetLength(1); mapctrPerIndex++)
                {
                    Console.Write(" " + challengeMap[mapIndex, mapctrPerIndex].ToString() + " ");

                }
                Console.Write("\n");
            }


        }
    }
}
