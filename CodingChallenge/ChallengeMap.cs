using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingChallenge
{
    public class ChallengeMap
    {
        public int MaxCountX { get; set; }
        public int MaxCountY { get; set; }
        public int[,] challengeMap;

        public ChallengeMap(int MaxCountX, int MaxCountY)
        {
            this.MaxCountX = MaxCountX;
            this.MaxCountY = MaxCountY;
            challengeMap = new int[this.MaxCountX, this.MaxCountY];
        }

        public void Create()
        {
            
            int valuePerBox = 1500;

            //Populating the ChallengeMap with data
            for (int mapIndex_X = 0; mapIndex_X < MaxCountX; mapIndex_X++)
            {
                for (int mapIndex_Y = 0; mapIndex_Y < MaxCountY; mapIndex_Y++)
                {
                    // *** ValuePerBox must be between 0 to 1500
                    valuePerBox--;

                    if (valuePerBox == 0)
                        valuePerBox = 1499;
                    //****

                    challengeMap[mapIndex_X, mapIndex_Y] = valuePerBox;
                }
            }
        }

        public void Display()
        {
            //DISPLAYING MAP
            for (int mapIndex_X = 0; mapIndex_X < MaxCountX; mapIndex_X++)
            {
                for (int mapIndex_Y = 0; mapIndex_Y < MaxCountY; mapIndex_Y++)
                {
                    Console.Write(" " + challengeMap[mapIndex_X, mapIndex_Y].ToString() + " ");

                }
                Console.Write("\n");
            }
        }
    }
}
