using Xunit;

namespace _MarsRover
{
    public class RoverTests
    {
        private readonly Direction _directionSut;
        private readonly Plateau _plateauSut;
        private readonly MoveValidator _moveValidatorSut;

        public RoverTests()
        {
            _directionSut = new Direction();
            _plateauSut = new Plateau();
            _moveValidatorSut = new MoveValidator(_directionSut, _plateauSut);
        }

        [Theory, InlineData("N", 0), InlineData("N", 360), InlineData("E", 90), InlineData("S", 180),
         InlineData("W", 270)]
        public void GetCardinalHeadingTheory(string expected, int heading)
        {
            Assert.Equal(expected, _directionSut.GetCardinalHeading(heading));
        }

        [Theory, InlineData(0, "N"), InlineData(0, ""), InlineData(90, "E"), InlineData(180, "S"),
         InlineData(270,"W")]
        public void GetHeadingDegreesTheory(int expected, string heading)
        {
            Assert.Equal(expected, _directionSut.GetHeadingDegrees(heading));
        }

        
        [Theory, InlineData(9, 9)]
        public void InitGridTheory(int x, int y)
        {
            var expected = new Plateau(x, y);
            Assert.True(expected.GetType() == typeof(Plateau));
            Assert.True(expected.LenX == x+1);
            Assert.True(expected.LenY == y+1);
        }
    }
}