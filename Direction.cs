namespace Platform45_MarsRover
{
    public class Direction
    {
        public int GetHeadingDegrees(string heading) =>
            heading switch
            {
                "N" => 0,
                "E" => 90,
                "S" => 180,
                "W" => 270,
                _ => 0
            };

        public string GetCardinalHeading(int heading) =>
            heading switch
            {
                0 => "N",
                90 => "E",
                180 => "S",
                270 => "W",
                _ => "N"
            };
    }
}