using FluentAssertions;
using Microsoft.CSharp.RuntimeBinder;
using Services.NhlData.Mappers;
using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakePlayerBioResponse;

namespace ServicesTests.UnitTests.NhlData.MapperTests
{
    [TestClass]
    public class MapPlayerResponseToPositionTests
    {
        [TestMethod]
        public void CallToCut_WithInvalidResponse_ShouldReturnNull()
        {
            dynamic message = new FakePlayerBioData();
            message.people[0].primaryPosition.abbreviation = null;

            string position = MapPlayerBioResponseToPositionStr.Map(message);

            position.Should().BeNull();

        }
        [TestMethod]
        public void CallToCut_WithValidResponse_ShouldReturnCorrectName()
        {
            string expectedPrimaryPosition = "RW";

            dynamic message = new FakePlayerBioData();
            message.people[0].primaryPosition.abbreviation = expectedPrimaryPosition;

            string position = MapPlayerBioResponseToPositionStr.Map(message);

            position.Should().Be(expectedPrimaryPosition);
        }
        [TestMethod]
        public void CallToCut_WithBasicResponse_ShouldThrowError()
        {
            dynamic message = "";

            Action testMap = () => MapPlayerBioResponseToPositionStr.Map(message);

            Assert.ThrowsException<RuntimeBinderException>(testMap);
        }
    }
}
