using Entities.Models;
using Entities.Types.Mappers;
using FluentAssertions;

namespace EntitiesTests.UnitTests.TypesTests.Mappers
{
    [TestClass]
    public class MapPositionStrToPositionTests
    {
        [TestMethod]
        public void CallToCut_WithGoalie_ShouldGetCorrectPosition()
        {
            var expectedPosition = POSITION.Goalie;
            var position = "G";

            var receivedPosition = MapPositionStrToPosition.Map(position);

            receivedPosition.Should().Be(expectedPosition);
        }
        [TestMethod]
        public void CallToCut_WithLeftWing_ShouldGetCorrectPosition()
        {
            var expectedPosition = POSITION.LeftWing;
            var position = "LW";

            var receivedPosition = MapPositionStrToPosition.Map(position);

            receivedPosition.Should().Be(expectedPosition);
        }
        [TestMethod]
        public void CallToCut_WithRightWing_ShouldGetCorrectPosition()
        {
            var expectedPosition = POSITION.RightWing;
            var position = "RW";

            var receivedPosition = MapPositionStrToPosition.Map(position);

            receivedPosition.Should().Be(expectedPosition);
        }
        [TestMethod]
        public void CallToCut_WithCenter_ShouldGetCorrectPosition()
        {
            var expectedPosition = POSITION.Center;
            var position = "C";

            var receivedPosition = MapPositionStrToPosition.Map(position);

            receivedPosition.Should().Be(expectedPosition);
        }
        [TestMethod]
        public void CallToCut_WithDefenseman_ShouldGetCorrectPosition()
        {
            var expectedPosition = POSITION.Defenseman;
            var position = "D";

            var receivedPosition = MapPositionStrToPosition.Map(position);

            receivedPosition.Should().Be(expectedPosition);
        }
        [TestMethod]
        public void CallToCut_WithEmptyPositionString_ShouldGetCorrectPosition()
        {
            var expectedPosition = POSITION.LeftWing;
            var position = "";

            var receivedPosition = MapPositionStrToPosition.Map(position);

            receivedPosition.Should().Be(expectedPosition);
        }
        [TestMethod]
        public void CallToCut_WithInvalidPositionString_ShouldGetCorrectPosition()
        {
            var expectedPosition = POSITION.LeftWing;
            var position = "a;sdfhkl";

            var receivedPosition = MapPositionStrToPosition.Map(position);

            receivedPosition.Should().Be(expectedPosition);
        }
    }
}
