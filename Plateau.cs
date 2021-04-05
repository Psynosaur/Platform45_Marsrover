using System.Collections.Generic;

namespace Platform45_MarsRover
{
    public class Plateau
    {
        // Our grid/plateau dimensions
        public int LenX;

        public int LenY;

        // List of parked rovers
        public readonly List<Rover> ParkedRovers = new List<Rover>();

        public int[,] InitGrid(int upperRightX, int upperRightY)
        {
            // we add one to each coords since they start at 0,0
            var grid = new int[upperRightX + 1, upperRightY + 1];
            LenX = grid.GetLength(0);
            LenY = grid.GetLength(1);
            return grid;
        }
    }
}