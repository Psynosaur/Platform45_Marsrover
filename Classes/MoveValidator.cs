using System;

namespace _MarsRover.Classes
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
            return x >= 0 && x < _plateau.LenX && y >= 0 && y < _plateau.LenY && !RoverCollides(x,y);
        }

        public bool RoverCollides(int x, int y)
        {
            var collider = false;
            foreach (var o in _plateau.ParkedRovers)
            {
                if (o.X != x || o.Y != y) continue;
                collider = true;
                break;
            }

            return collider;
        }
        // Collision and bounds check in same function :)
        public bool CanMove(Rover r, int x, int y, int[,] plateau)
        {
            if (InitOrMovePosition(x, y))
            {
                return CollisionCheck(r, x, y, plateau);
            }

            r.Errors++;
            Console.WriteLine(
                $"[ERROR] : Moving rover {r.Number} at x: {r.X} y: {r.Y} with heading '{_direction.GetCardinalHeading(r.H)}' would place " +
                $"it out of bounds!!!  skipping '{_direction.GetCardinalHeading(r.H)}' move command");
            return false;
        }

        public bool CollisionCheck(Rover r, int x, int y, int[,] plateau)
        {
            if (RoverCollides(x, y))
            {
                r.Errors++;
                var rover = Collider(x, y);

                if (rover != null)
                    Console.WriteLine(
                        $"[ERROR] : Collision detected with rover {rover.Number}!!! skipping '{_direction.GetCardinalHeading(r.H)}' move command");
                return false;
            }

            // Claim the discovery for this coordinates
            if (plateau[x, y] == 0) plateau[x, y] = r.Number;
            return true;
        }

        public Rover? Collider(int x, int y)
        {
            Rover? collider = null;
            foreach (var o in _plateau.ParkedRovers)
            {
                if (o.X != x || o.Y != y) continue;
                collider = o;
                break;
            }

            return collider;
        }
    }
}