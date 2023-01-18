using FluentAssertions;
using Microsoft.CSharp.RuntimeBinder;
using Services.NhlData.Mappers;
using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakePlayerBioResponse;

namespace ServicesTests.UnitTests.NhlData.MapperTests
{
    [TestClass]
    public class MapPlayerBioResponseToNameTests
    {
        [TestMethod]
        public void CallToCut_WithNullResponse_ShouldReturnNull()
        {
            dynamic message = new FakePlayerBioData();
            message.people[0].fullName = null;

            string fullName = MapPlayerBioResponseToName.Map(message);

            fullName.Should().BeNull();
        }
        [TestMethod]
        public void CallToCut_WithValidResponse_ShouldReturnCorrectName()
        {
            string expectedName = "Cole Titze";

            dynamic message = new FakePlayerBioData();
            message.people[0].fullName = expectedName;

            string fullName = MapPlayerBioResponseToName.Map(message);

            fullName.Should().Be(expectedName);
        }
        [TestMethod]
        public void CallToCut_WithEmptyResponse_ShouldThrowError()
        {
            dynamic message = "";

            Action testMap = () => MapPlayerBioResponseToName.Map(message);

            Assert.ThrowsException<RuntimeBinderException>(testMap);
        }
    }
}
