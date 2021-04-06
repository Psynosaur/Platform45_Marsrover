using System;
using System.Linq;

namespace _MarsRover
{
    public class MoveValidator
    {
        private readonly Direction _direction;
        private readonly Plateau _plateau;

        public MoveValidator(Direction direction, Plateau plateau)
        {
            _direction = direction;
            _plateau = plateau;
        }

        public bool InitOrMovePosition(int x, int y)
        {
            return x >= 0 && x < _plateau.LenX && y >= 0 && y < _plateau.LenY;
        }

        public bool RoverMoveCollides(int x, int y)
        {
            var collider = _plateau.ParkedRovers.Any(o => o.X == x && o.Y == y);
            return collider;
        }
        // Collision and bounds check in same function :)
        public bool CanMove(Rover r, int x, int y, int[,] plateau)
        {
            if (InitOrMovePosition(x, y))
            {
                if (RoverMoveCollides(x, y))
                {
                    r.Errors++;
                    var collider = _plateau.ParkedRovers.FirstOrDefault(o => o.X == x && o.Y == y);
                    if (collider != null)
                        Console.WriteLine(
                            $"[ERROR] : Collision detected with rover {collider.Number}!!! skipping '{_direction.GetCardinalHeading(r.H)}' move command");
                    return false;
                }

                // Claim the discovery for this coordinates
                if (plateau[x, y] == 0) plateau[x, y] = r.Number;
                return true;
            }

            r.Errors++;
            Console.WriteLine(
                $"[ERROR] : Moving rover {r.Number} at x: {r.X} y: {r.Y} with heading '{_direction.GetCardinalHeading(r.H)}' would place " +
                $"it out of bounds!!!  skipping '{_direction.GetCardinalHeading(r.H)}' move command");
            return false;
        }
    }
}