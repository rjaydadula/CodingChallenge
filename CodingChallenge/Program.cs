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

            ChallengeMap map = new ChallengeMap(10,10);
            PathFinder pathFinder = new PathFinder();

            map.Create();
            map.Display();

            pathFinder.ProcessMap(map);

            Console.Write("\n\n");
            Console.WriteLine("Length Of Calculated Path: "+ pathFinder.LengthOfCalculatedPath);
            Console.WriteLine("Drop Of Calculated Path: " + pathFinder.DropOfCalculatedPath);
            Console.WriteLine("Calculated Path: " + pathFinder.CalculatedPath);

            Console.Read();

        }
    }
}
