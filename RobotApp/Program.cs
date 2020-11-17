using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RobotApp
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var basepath = AppDomain.CurrentDomain.BaseDirectory;
            var Sample = Path.GetFullPath(Path.Combine(basepath, @"..\..\..\Sample.txt"));
            var Sample1 = Path.GetFullPath(Path.Combine(basepath, @"..\..\..\Sample1.txt"));
            var Sample2 = Path.GetFullPath(Path.Combine(basepath, @"..\..\..\Sample2.txt"));
            Console.WriteLine("\n\nReading From Sample.txt \n\n");
            ReadFileAndStartJourney(Sample);
            Console.WriteLine("\n\nReading From Sample1.txt \n\n");
            ReadFileAndStartJourney(Sample1);
            Console.WriteLine("\n\nReading From Sample2.txt \n\n");
            ReadFileAndStartJourney(Sample2);


        }




        private static void ReadFileAndStartJourney(string filename)
        {
            List<string> journeys;
            try
            {
                journeys = File.ReadLines(filename).Where(x => !string.IsNullOrEmpty(x)).ToList();

            }
            catch (Exception)
            {
                throw new InvalidDataException("Invalid FileName");
            }

            // Loop through lines and read first 3 of them for Robot input.
            for (int i = 0; i < journeys.Count; i += 3)
            {
                // If it starts with 'O', consider it as an obstacle
                if (journeys[i].StartsWith("O"))
                {
                    var obstacle = journeys.Skip(i).Take(3).ToArray();
                    CreateObstacles(obstacle);
                    continue;
                }

                var journey = journeys.Skip(i).Take(3).ToArray();
                CreateRobot(journey);
            }

        }


        private static void CreateObstacles(string[] obstacles)
        {
            foreach (var obs in obstacles)
            {
                var coordinates = obs.Split(" ");
                Obstacles.Coordinates.Add(new Coordinate
                {
                    X = Convert.ToInt32(coordinates[1]),
                    Y = Convert.ToInt32(coordinates[2])
                });

            }
        }

        private static void CreateRobot(string[] journey)
        {
            var initialCoordinates = journey[0];
            var commands = journey[1];
            var expectedResult = journey[2];


            var robot = InitializeRobot(initialCoordinates);
            var actualResult = robot.startJourney(commands);

            if (actualResult.Contains("CRASHED"))
            {
                Console.WriteLine(actualResult);
                return;
            }


            if (expectedResult.Equals(actualResult))
            {
                Console.WriteLine("SUCCESS" + " " + actualResult);
            }
            else
            {
                Console.WriteLine("FAILURE" + " " + actualResult);
            }
        }


        private static Robot InitializeRobot(string initialCoordiantes)
        {
            var currentPosition = initialCoordiantes.Split(" ");

            var coordinates = new Coordinate()
            {
                X = Convert.ToInt32(currentPosition[0]),
                Y = Convert.ToInt32(currentPosition[1])

            };

            var direction = char.Parse(currentPosition[2]);

            return new Robot(coordinates, direction);

        }
    }
}
