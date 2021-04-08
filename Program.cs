using System;
using System.Collections.Generic;
using _MarsRover.Classes;

namespace _MarsRover
{
    public class Program
    {
        private const string TestMessage =
            "9 9\n" +
            "1 2 N\n" +
            "LMLMLMLMM\n" +
            "3 3 E\n" +
            "MMRMMRMRRM\n" +
            "0 0 E\n" +
            "MLMMMMLMLMLMLMMRMRMMLMM\n" +
            "9 9 W\n" +
            "MMMMMLMLMMMMMLMLMLMLMMMMRMRMMMMMML\n" +
            "7 7 W\n" +
            "MLMLMMMMMLMLMLMLMMMMRMRMMMMMMLLL\n" +
            "6 6 W\n" +
            "MLMMMMLMLMLMLMMRMRMMLMMLL\n" +
            "9 0 N\n" +
            "MMMMLMLMMMMRMRMMMMLMLMMRMRMMLMML\n" +
            "5 1 W\n" +
            "MLMMMMLMLMLMLMMRMRMMLMM";

        private static List<string> Pop(List<string> cmdList)
        {
            cmdList.RemoveAt(0);
            return cmdList;
        }

        private static string? PopLine(IReadOnlyList<string> cmdList)
        {
            string? firstLine = null;
            for (var i = 0; i < cmdList.Count;)
            {
                var s = cmdList[i];
                firstLine = s;
                break;
            }

            return firstLine;
        }

        private static void PrintPlateau(int[,] plateau)
        {
            for (var y = plateau.GetLength(1) - 1; y >= 0; y--)
            {
                for (var x = 0; x < plateau.GetLength(0); x++) Console.Write(plateau[x, y] + " ");
                Console.WriteLine();
            }
        }

        // Let NASA know how many pixels are unexplored within our plateau
        private static int UnexploredPixels(int[,] grid)
        {
            var cnt = 0;
            for (var i0 = 0; i0 < grid.GetLength(0); i0++)
            for (var i1 = 0; i1 < grid.GetLength(1); i1++)
            {
                var num = grid[i0, i1];
                if (num == 0) cnt++;
            }

            return cnt;
        }

        public static void Main()
        {
            try
            {
                // split our TestMessage into a 'stack' of strings
                var cmdList = new List<string>();
                foreach (var s in TestMessage.Split('\n')) cmdList.Add(s);

                // line one of the cmdList 'stack' is the upper coordinates of our plateau
                var firstLine = PopLine(cmdList);

                var upperCoords = firstLine?.Split(' ');

                // remove line one of the cmdList 'stack' since we've used it values
                cmdList = Pop(cmdList);

                // setup our plateau / matrix
                var plateau = new Plateau();
                if (upperCoords != null)
                    plateau = new Plateau(Convert.ToInt32(upperCoords[0]),
                        Convert.ToInt32(upperCoords[1]));
                var direction = new Direction();
                var validator = new MoveValidator(direction, plateau);

                // cmdList minus first line(5 5) used for plateau setup divided by 2 lines per rover 
                // >>> 1st line is rover 'start state'
                // >>> 2nd line is rover 'do stuff'
                var roverCount = cmdList.Count / 2;

                Console.WriteLine($"The test input has {roverCount} {(roverCount > 1 ? "rovers" : "rover")}");
                // Process rover commands synchronously per rover
                for (var i = 1; i <= roverCount; i++)
                {
                    // first line is used to initialize the rover with a starting position
                    var initCommandLine = PopLine(cmdList)?.Split(" ");

                    // remove the item from cmdList 'stack' since we've used it values
                    cmdList = Pop(cmdList);

                    // Init a rover class
                    if (initCommandLine != null)
                    {
                        var initX = Convert.ToInt32(initCommandLine[0]);
                        var initY = Convert.ToInt32(initCommandLine[1]);
                        var heading = direction.GetHeadingDegrees(initCommandLine[2]);
                        if (!validator.InitOrMovePosition(initX, initY))
                        {
                            var collider = validator.Collider(initX, initY);
                            Console.WriteLine(
                                $"\nRover {i} deployment at {initX} {initY} {direction.GetCardinalHeading(heading)} " +
                                $"{(collider != null ? $"would collide with Rover {collider.Number}" : "is out of bounds")} trying the next one");
                            // remove the doStuffCommandLine from the cmdList 'stack' since we've tried to init the rover but it was invalid
                            cmdList = Pop(cmdList);
                            // and move on to the next rover without doing stuff
                            continue;
                        }

                        var rover = new Rover(initX, initY, heading, validator);
                        // set the start position as explored too
                        plateau.Grid[rover.X, rover.Y] = i;
                        Console.WriteLine($"\nDeploying rover {i} on validated coordinates" +
                                          $" {rover.X} {rover.Y} {direction.GetCardinalHeading(rover.H)} ");
                        // after removing the rover init line from the cmdList 'stack', we have the "do stuff" line
                        // for this initialized rover
                        var doStuffCommandLine = PopLine(cmdList)?.ToCharArray();
                        if (doStuffCommandLine != null)
                        {
                            rover.CmdCount = doStuffCommandLine.Length;
                            rover.Number = i;
                            CommandProcessor(doStuffCommandLine, rover, plateau);
                        }

                        // remove the commandLine from the cmdList 'stack' since we've used it
                        cmdList = Pop(cmdList);
                        Console.WriteLine($"The state of the plateau after rover {i} finished its commands\n");

                        // Use this for collision detection
                        plateau.ParkedRovers.Add(rover);
                    }

                    // Print the rovers progress
                    PrintPlateau(plateau.Grid);
                }

                var cnt = UnexploredPixels(plateau.Grid);

                Console.WriteLine($"\nUnexplored pixels in our plateau {cnt}");
                // Summarize rover states, again
                var index = 1;
                foreach (var rover in plateau.ParkedRovers)
                {
                    Console.WriteLine(
                        $"\nRover {index} \nOutput: {rover.X} {rover.Y} {direction.GetCardinalHeading(rover.H)}" +
                        $"\nProcessed : {rover.CmdCount} commands with {(rover.Errors > 0 ? $"{rover.Errors} errors" : "no errors")} ");
                    index++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void CommandProcessor(char[] doStuffCommandLine, Rover rover, Plateau plateau)
        {
            foreach (var c in doStuffCommandLine)
                switch (c)
                {
                    case 'L':
                    case 'R':
                        rover.Turn(rover, c);
                        break;
                    default:
                        rover.Move(rover, rover.H, plateau.Grid);
                        break;
                }
        }
    }
}