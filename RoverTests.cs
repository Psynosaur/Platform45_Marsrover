using Xunit;

namespace Platform45_MarsRover
{
    public class RoverTests
    {
        private readonly Direction _sut;

        public RoverTests()
        {
            _sut = new Direction();
        }
       
        [Theory]
        [InlineData("N", 0)]
        [InlineData("E", 90)]
        [InlineData("S", 180)]
        [InlineData("W", 270)]
        public void GetCardinalHeadingTheory(string expected, int heading)
        {
            Assert.Equal(expected, _sut.GetCardinalHeading(heading));
        }
    }
}