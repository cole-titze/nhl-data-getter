using FluentAssertions;
using Microsoft.CSharp.RuntimeBinder;
using Services.NhlData;
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
            int seasonYear = 20202021;
            int nonSeasonYear = 20222023;

            dynamic message = new FakeScheduleData();
            message.data = new List<FakeData>() { new FakeData(), new FakeData() };
            message.data[0].totalRegularSeasonGames = 200;
            message.data[0].id = nonSeasonYear;

            message.data[1].id = seasonYear;
            message.data[1].totalRegularSeasonGames = exceptedSeasonGameCount;

            int totalItems = MapScheduleToGameCount.Map(message, seasonYear);

            totalItems.Should().Be(exceptedSeasonGameCount);
        }
    }
}
