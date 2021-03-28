using System;
using System.Collections.Generic;
using System.Linq;

namespace Platform45_MarsRover
{
    internal class Program
    {
        private static readonly string TestMessage =
            "5 5\n1 2 N\nLMLMLMLMM\n3 3 E\nMMRMMRMRRM\n0 0 E\nMLMMMMRMRMMLMM\n5 5 W\nMMMMMLMLMMMMMMMRMRMMMMMM";

        // Our grid/plateau dimensions
        private static int _lenX;

        private static int _lenY;

        // List of parked rovers
        private static readonly List<Rover> ParkedRovers = new List<Rover>();

        private static int GetHeadingDegrees(string heading)
        {
            return heading switch
            {
                "N" => 0,
                "E" => 90,
                "S" => 180,
                "W" => 270,
                _ => 0
            };
        }

        private static string GetCardinalHeading(int heading)
        {
            return heading switch
            {
                0 => "N",
                90 => "E",
                180 => "S",
                270 => "W",
                _ => "N"
            };
        }

        private static int[,] InitGrid(int upperRightX, int upperRightY)
        {
            // we add one to each coords since they start at 0,0
            var grid = new int[upperRightX + 1, upperRightY + 1];
            _lenX = grid.GetLength(0);
            _lenY = grid.GetLength(1);
            return grid;
        }

        private static bool CheckRoverInitOrMovePosition(int x, int y)
        {
            return x >= 0 && x < _lenX && y >= 0 && y < _lenY;
        }

        private static bool CheckRoverMoveForCollision(int x, int y, IEnumerable<Rover> rovers)
        {
            var checkCollision = rovers.Any(o => o.X == x && o.Y == y);
            return checkCollision;
        }


        // Set the number at coordinate in the plateau to the rover number that explored it first
        private static void MoveRover(Rover r, int heading, int[,] plateau)
        {
            // We can check here that the move would not result in the rover being out of bounds
            // For all intends and purposes, Im going to discard a move instruction either if it
            // would result in the rover being out of bounds or it would crash into another rover
            switch (heading)
            {
                case 0:
                    if (CheckRoverInitOrMovePosition(r.X, r.Y + 1))
                    {
                        if (CheckRoverMoveForCollision(r.X, r.Y + 1, ParkedRovers))
                        {
                            r.Errors++;
                            var collider = ParkedRovers.FirstOrDefault(o => o.X == r.X && o.Y == r.Y + 1);
                            if (collider != null)
                                Console.WriteLine(
                                    $"[ERROR] : Collision detected with rover {collider.Number}!!! skipping '{GetCardinalHeading(r.H)}' move command");
                        }
                        else
                        {
                            r.Y += 1;
                        }

                        // Claim the discovery for this coordinates
                        if (plateau[r.X, r.Y] == 0) plateau[r.X, r.Y] = r.Number;
                    }
                    else
                    {
                        r.Errors++;
                        Console.WriteLine(
                            $"[ERROR] : Moving rover {r.Number} at x: {r.X} y: {r.Y} with heading '{GetCardinalHeading(r.H)}' would place " +
                            $"it out of bounds!!!  skipping '{GetCardinalHeading(r.H)}' move command");
                    }

                    break;
                case 90:
                    if (CheckRoverInitOrMovePosition(r.X + 1, r.Y))
                    {
                        if (CheckRoverMoveForCollision(r.X + 1, r.Y, ParkedRovers))
                        {
                            r.Errors++;
                            var collider = ParkedRovers.FirstOrDefault(o => o.X == r.X + 1 && o.Y == r.Y);
                            if (collider != null)
                                Console.WriteLine(
                                    $"[ERROR] : Collision detected with rover {collider.Number}!!! skipping '{GetCardinalHeading(r.H)}' move command");
                        }
                        else
                        {
                            r.X += 1;
                        }

                        // Claim the discovery for this coordinates
                        if (plateau[r.X, r.Y] == 0) plateau[r.X, r.Y] = r.Number;
                    }
                    else
                    {
                        r.Errors++;
                        Console.WriteLine(
                            $"[ERROR] : Moving rover {r.Number} at x: {r.X} y: {r.Y} with heading '{GetCardinalHeading(r.H)}' would place " +
                            $"it out of bounds!!!  skipping '{GetCardinalHeading(r.H)}' move command");
                    }

                    break;
                case 180:
                    if (CheckRoverInitOrMovePosition(r.X, r.Y - 1))
                    {
                        if (CheckRoverMoveForCollision(r.X, r.Y - 1, ParkedRovers))
                        {
                            r.Errors++;
                            var collider = ParkedRovers.FirstOrDefault(o => o.X == r.X && o.Y == r.Y - 1);
                            if (collider != null)
                                Console.WriteLine(
                                    $"[ERROR] : Collision detected with rover {collider.Number}!!! skipping '{GetCardinalHeading(r.H)}' move command");
                        }
                        else
                        {
                            r.Y -= 1;
                        }

                        // Claim the discovery for this coordinates
                        if (plateau[r.X, r.Y] == 0) plateau[r.X, r.Y] = r.Number;
                    }
                    else
                    {
                        r.Errors++;
                        Console.WriteLine(
                            $"[ERROR] : Moving rover {r.Number} at x: {r.X} y: {r.Y} with heading '{GetCardinalHeading(r.H)}' would place " +
                            $"it out of bounds!!!  skipping '{GetCardinalHeading(r.H)}' move command");
                    }

                    break;
                case 270:
                    if (CheckRoverInitOrMovePosition(r.X - 1, r.Y))
                    {
                        if (CheckRoverMoveForCollision(r.X - 1, r.Y, ParkedRovers))
                        {
                            r.Errors++;
                            var collider = ParkedRovers.FirstOrDefault(o => o.X == r.X - 1 && o.Y == r.Y);
                            if (collider != null)
                                Console.WriteLine(
                                    $"[ERROR] : Collision detected with rover {collider.Number}!!! skipping '{GetCardinalHeading(r.H)}' move command");
                        }
                        else
                        {
                            r.X -= 1;
                        }

                        // Claim the discovery for this coordinates
                        if (plateau[r.X, r.Y] == 0) plateau[r.X, r.Y] = r.Number;
                    }
                    else
                    {
                        r.Errors++;
                        Console.WriteLine(
                            $"[ERROR] : Moving rover {r.Number} at x: {r.X} y: {r.Y} with heading '{GetCardinalHeading(r.H)}' would place " +
                            $"it out of bounds!!!  skipping '{GetCardinalHeading(r.H)}' move command");
                    }

                    break;
            }
        }

        private static void TurnRover(Rover r, char turn)
        {
            if (turn == 'R')
            {
                r.H += 90;
                // when h is 360°, we set it to 0° so that we get 90°(E) when adding 90° next time
                if (r.H == 360) r.H = 0;
            }
            else
            {
                // when h is 0°, we set it to 360° so that we get 270°(W) when subtracting 90°
                if (r.H == 0) r.H = 360;
                r.H -= 90;
            }
        }

        private static void Main()
        {
            try
            {
                // split our TestMessage into a 'stack' of strings
                var cmdList = TestMessage.Split('\n').ToList();

                // line one of the cmdList 'stack' is the upper coordinates of our plateau
                string[] upperCoords;
                upperCoords = cmdList.FirstOrDefault()?.Split(' ');

                // remove line one of the cmdList 'stack' since we've used it values
                cmdList = cmdList.Skip(1).ToList();

                // setup our plateau / matrix
                int[,] plateau = { };
                if (upperCoords != null)
                    plateau = InitGrid(
                        Convert.ToInt32(upperCoords[0]),
                        Convert.ToInt32(upperCoords[1]));

                // cmdList minus first line(5 5) used for plateau setup divided by 2 lines per rover 
                // >>> 1st line is rover 'start state'
                // >>> 2nd line is rover 'do stuff'
                var roverCount = cmdList.Count / 2;

                Console.WriteLine($"The test input has {roverCount} {(roverCount > 1 ? "rovers" : "rover")}");
                // Process rover commands synchronously per rover
                for (var i = 1; i <= roverCount; i++)
                {
                    // first line is used to initialize the rover with a starting position
                    var initCommandLine = cmdList.FirstOrDefault()?.Split(" ");

                    // remove the item from cmdList 'stack' since we've used it values
                    cmdList = cmdList.Skip(1).ToList();

                    // Init a rover class
                    if (initCommandLine != null)
                    {
                        var initX = Convert.ToInt32(initCommandLine[0]);
                        var initY = Convert.ToInt32(initCommandLine[1]);
                        var heading = GetHeadingDegrees(initCommandLine[2]);
                        Rover rover;
                        if (CheckRoverInitOrMovePosition(initX, initY))
                        {
                            rover = new Rover(initX, initY, heading);
                            // set the start position as explored too
                            plateau[rover.X, rover.Y] = i;
                            Console.WriteLine($"\nDeploying rover {i} on validated coordinates" +
                                              $" {rover.X} {rover.Y} {GetCardinalHeading(rover.H)} ");
                        }
                        else
                        {
                            Console.WriteLine($"Rover {i} out of bounds, trying to deploy the next one");
                            // remove the commandLine from the cmdList 'stack' since we've used it and it was invalid
                            cmdList = cmdList.Skip(1).ToList();
                            // and move on to the next rover without doing stuff
                            continue;
                        }

                        // after removing the rover init line from the cmdList 'stack', we have the "do stuff" line
                        // for this initialized rover
                        var doStuffCommandLine = cmdList.FirstOrDefault()?.ToCharArray();
                        if (doStuffCommandLine != null)
                        {
                            rover.CmdCount = doStuffCommandLine.Length;
                            rover.Number = i;
                            foreach (var c in doStuffCommandLine)
                                if (c == 'L' || c == 'R')
                                    TurnRover(rover, c);
                                else MoveRover(rover, rover.H, plateau);
                        }

                        // remove the commandLine from the cmdList 'stack' since we've used it
                        cmdList = cmdList.Skip(1).ToList();
                        Console.WriteLine($"The state of the plateau after rover {i} finished its commands\n");

                        // Use this for collision detection
                        ParkedRovers.Add(rover);
                    }

                    // Print the rovers progress
                    for (var x = plateau.GetLength(1) - 1; x >= 0; x--)
                    {
                        for (var y = 0; y < plateau.GetLength(0); y++) Console.Write(plateau[y, x] + " ");
                        Console.WriteLine();
                    }
                }

                // Let NASA know how many pixels are unexplored within our plateau
                var cnt = plateau.Cast<int>().Count(num => num == 0);
                Console.WriteLine($"\nUnexplored pixels in our plateau {cnt}");
                // Summarize rover states, again
                var index = 1;
                foreach (var rover in ParkedRovers)
                {
                    Console.WriteLine(
                        $"\nRover {index} \nOutput: {rover.X} {rover.Y} {GetCardinalHeading(rover.H)}" +
                        $"\nProcessed : {rover.CmdCount} commands with {(rover.Errors > 0 ? $"{rover.Errors} errors" : "no errors")} ");
                    index++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private class Rover
        {
            public Rover(int x, int y, int h)
            {
                X = x;
                Y = y;
                H = h;
            }

            public int Number { get; internal set; }
            public int X { get; internal set; }
            public int Y { get; internal set; }
            public int H { get; internal set; }
            public int CmdCount { get; set; }
            public int Errors { get; set; }
        }
    }
}