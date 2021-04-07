using System.Runtime.InteropServices;
using _MarsRover.Classes;
using Xunit;

namespace _MarsRover.Tests
{
    public class RoverTests
    {
        private readonly Direction _directionSut;
        private readonly MoveValidator _moveValidatorSut;
        private readonly Plateau _plateauSut;

        public RoverTests()
        {
            _directionSut = new Direction();
            _plateauSut = new Plateau(9, 9);
            _moveValidatorSut = new MoveValidator(_directionSut, _plateauSut);
        }

        [Theory]
        [InlineData("N", 0)]
        [InlineData("N", 360)]
        [InlineData("E", 90)]
        [InlineData("S", 180)]
        [InlineData("W", 270)]
        public void GetCardinalHeadingTheory(string expected, int heading)
        {
            Assert.Equal(expected, _directionSut.GetCardinalHeading(heading));
        }

        [Theory]
        [InlineData(0, "N")]
        [InlineData(0, "")]
        [InlineData(90, "E")]
        [InlineData(180, "S")]
        [InlineData(270, "W")]
        public void GetHeadingDegreesTheory(int expected, string heading)
        {
            Assert.Equal(expected, _directionSut.GetHeadingDegrees(heading));
        }


        [Theory]
        [InlineData(9, 10)]
        [InlineData(3, 7)]
        [InlineData(4, 8)]
        public void InitialisePlateauTheory(int x, int y)
        {
            // We test our plateau initialisation 
            var expected = new Plateau(x, y);
            Assert.Equal(expected.Grid, _plateauSut.InitGrid(x, y));
            // We make sure it is the correct type and dimensions
            Assert.True(expected.Grid.GetType() == typeof(int[,]));
            Assert.True(expected.GetType() == typeof(Plateau));
            Assert.True(expected.LenX == x + 1);
            Assert.True(expected.LenY == y + 1);
            Assert.True(expected.ParkedRovers.Count == 0);
        }

        [Theory]
        [InlineData(true, 1, 1)]
        [InlineData(true, 3, 3)]
        [InlineData(false, 9, 10)]
        [InlineData(false, 10, 10)]
        public void InitOrMovePositionTheory(bool expected, int x, int y)
        {
            Assert.Equal(expected, _moveValidatorSut.InitOrMovePosition(x, y));
        }
    }
}