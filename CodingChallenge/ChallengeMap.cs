using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingChallenge
{
    public class ChallengeMap
    {
        public int MaxCountX { get; set; }
        public int MaxCountY { get; set; }
        public string[,] challengeMap;

        public void Create(string mapName)
        {
            string _filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            _filePath = Directory.GetParent(Directory.GetParent(_filePath).FullName).FullName;
            _filePath += @"\"+ mapName;
            if (File.Exists(_filePath))
            {
                try
                {
                    using (StreamReader streamReader = new StreamReader(_filePath))
                    {
                        string reader = streamReader.ReadToEnd();

                        using (StringReader strReader = new StringReader(reader))
                        {
                            string line = string.Empty;
                            int lineCounter = 0;
                            while ((line = strReader.ReadLine()) != null)
                            {
                                if (lineCounter > 0)
                                {
                                    //Populating the ChallengeMap with data

                                    string[] fileData = line.Split(' ');

                                    int mapIndex_Y = 0;
                                    int mapIndex_X = lineCounter - 1;
                                    foreach (string data in fileData)
                                    {
                                        if (mapIndex_Y > this.MaxCountY - 1)
                                            break;

                                        challengeMap[mapIndex_X, mapIndex_Y] = data.Trim();
                                        mapIndex_Y++;
                                    }

                                }
                                else
                                {
                                    string[] XYData = line.Split(' ');
                                    this.MaxCountX = int.Parse(XYData[0]);
                                    this.MaxCountY = int.Parse(XYData[1]);
                                    challengeMap = new string[this.MaxCountX, this.MaxCountY];
                                }

                                lineCounter++;
                            }
                        }
                    }
                }
                catch
                {
                    throw new Exception("Invalid Map!");
                }
            }
            else
            {
                Console.WriteLine("map.txt not Found!");
            }
        }
    }
}
