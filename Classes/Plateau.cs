using System.Collections.Generic;

namespace _MarsRover
{
    public class Plateau
    {
        public Plateau(int x = 0, int y = 0)
        {
            Grid = InitGrid(x, y);
        }
        // Our grid/plateau dimensions
        public int LenX;

        public int LenY;

        public int[,] Grid;

        // List of parked rovers
        public readonly List<Rover> ParkedRovers = new List<Rover>();

        public int[,] InitGrid(int upperRightX, int upperRightY)
        {
            // we add one to each coords since they start at 0,0
            Grid = new int[upperRightX + 1, upperRightY + 1];
            LenX = Grid.GetLength(0);
            LenY = Grid.GetLength(1);
            return Grid;
        }
    }
}