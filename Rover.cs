namespace Platform45_MarsRover
{
    public class Rover
    {
        private readonly CommandValidator _validator;
        public Rover(int x, int y, int h, CommandValidator validator)
        {
            X = x;
            Y = y;
            H = h;
            _validator = validator;
        }

        public int Number { get; internal set; }
        public int X { get; internal set; }
        public int Y { get; internal set; }
        public int H { get; internal set; }
        public int CmdCount { get; set; }
        public int Errors { get; set; }
        
        // Set the number at coordinate in the plateau to the rover number that explored it first

        public void Move(Rover r, int heading, int[,] plateau)
        {
            // We can check here that the move would not result in the rover being out of bounds
            // For all intends and purposes, Im going to discard a move instruction either if it
            // would result in the rover being out of bounds or it would crash into another rover
            switch (heading)
            {
                case 0:
                    if (_validator.CanMove(r, r.X, r.Y + 1, plateau)) r.Y++;
                    break;
                case 90:
                    if (_validator.CanMove(r, r.X + 1, r.Y, plateau)) r.X++;
                    break;
                case 180:
                    if (_validator.CanMove(r, r.X, r.Y - 1, plateau)) r.Y--;
                    break;
                case 270:
                    if (_validator.CanMove(r, r.X - 1, r.Y, plateau)) r.X--;
                    break;
            }
        }

        public void Turn(Rover r, char turn)
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
    }
}