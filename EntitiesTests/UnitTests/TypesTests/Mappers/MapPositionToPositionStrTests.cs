using Entities.Models;
using Entities.Types.Mappers;
using FluentAssertions;

namespace EntitiesTests.UnitTests.TypesTests.Mappers
{
    [TestClass]
    public class MapPositionToPositionStrTests
    {
        [TestMethod]
        public void CallToCut_WithGoalie_ShouldGetCorrectString()
        {
            var position = POSITION.Goalie;
            var expectedPosition = "G";

            var receivedPosition = MapPositionToPositionStr.Map(position);

            receivedPosition.Should().Be(expectedPosition);
        }
        [TestMethod]
        public void CallToCut_WithLeftWing_ShouldGetCorrectString()
        {
            var position = POSITION.LeftWing;
            var expectedPosition = "L";

            var receivedPosition = MapPositionToPositionStr.Map(position);

            receivedPosition.Should().Be(expectedPosition);
        }
        [TestMethod]
        public void CallToCut_WithRightWing_ShouldGetCorrectString()
        {
            var position = POSITION.RightWing;
            var expectedPosition = "R";

            var receivedPosition = MapPositionToPositionStr.Map(position);

            receivedPosition.Should().Be(expectedPosition);
        }
        [TestMethod]
        public void CallToCut_WithCenter_ShouldGetCorrectString()
        {
            var position = POSITION.Center;
            var expectedPosition = "C";

            var receivedPosition = MapPositionToPositionStr.Map(position);

            receivedPosition.Should().Be(expectedPosition);
        }
        [TestMethod]
        public void CallToCut_WithDefenseman_ShouldGetCorrectString()
        {
            var position = POSITION.Defenseman;
            var expectedPosition = "D";

            var receivedPosition = MapPositionToPositionStr.Map(position);

            receivedPosition.Should().Be(expectedPosition);
        }
    }
}
