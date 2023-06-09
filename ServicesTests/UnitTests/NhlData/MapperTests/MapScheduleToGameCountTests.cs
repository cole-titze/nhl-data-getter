using FluentAssertions;
using Microsoft.CSharp.RuntimeBinder;
using Services.NhlData.Mappers;
using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakeScheduleResponse;

namespace ServicesTests.UnitTests.NhlData.MapperTests
{
    [TestClass]
    public class MapScheduleToGameCountTests
    {
        [TestMethod]
        public void CallToCut_WithEmptyResponse_ShouldThrowError()
        {
            dynamic message = "";
            Action testMap = () => MapPlayerBioResponseToName.Map(message);

            Assert.ThrowsException<RuntimeBinderException>(testMap);
        }
        [TestMethod]
        public void CallToCut_WithInvalidResponse_ShouldThrowError()
        {
            dynamic message = new FakeScheduleData();
            message.totalGames = null;

            Action testMap = () => MapPlayerBioResponseToName.Map(message);

            Assert.ThrowsException<RuntimeBinderException>(testMap);
        }
        [TestMethod]
        public void CallToCut_WithValidResponse_ShouldGetGameCount()
        {
            int exceptedSeasonGameCount = 1203;

            dynamic message = new FakeScheduleData();
            message.totalGames = exceptedSeasonGameCount;

            int totalItems = MapScheduleToGameCount.Map(message);

            totalItems.Should().Be(exceptedSeasonGameCount);
        }
    }
}
