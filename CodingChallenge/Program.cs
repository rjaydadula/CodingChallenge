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

            ChallengeMap map = new ChallengeMap(1000,1000);
            map.Create();

            PathFinder pathFinder = new PathFinder();
            pathFinder.SearchPath(map);

            if (pathFinder.CalculatedPath != null && pathFinder.CalculatedPath.Trim() != string.Empty)
            {
                Console.WriteLine("Length Of Calculated Path: " + pathFinder.LengthOfCalculatedPath);
                Console.WriteLine("Drop Of Calculated Path: " + pathFinder.DropOfCalculatedPath);
                Console.WriteLine("Calculated Path: " + pathFinder.CalculatedPath);
                
            }
            else
            {
                Console.WriteLine("No Possible Path Down!");
            }

            Console.Read();

        }
    }
}
