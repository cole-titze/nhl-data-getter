using FluentAssertions;
using FakeItEasy;
using Services.NhlData;

namespace ServicesTests.UnitTests.NhlData
{
    [TestClass]
    public class NhlDataGetterTests
    {
        [TestMethod]
        public void CallToGetGameId_ShouldReturnCorrectGameId()
        {
            int seasonStartYear = 2021;
            int gameNumber = 200;
            int expectedGameId = 2021020200;

            var gameId = NhlDataGetter.GetGameId(seasonStartYear, gameNumber);
        
            gameId.Should().Be(expectedGameId);
        }
        [TestMethod]
        public void CallToGetFullSeasonId_ShouldReturnCorrectSeasonId()
        {
            int seasonStartYear = 2021;
            int expectedSeasonId = 20212022;

            var gameId = NhlDataGetter.GetFullSeasonId(seasonStartYear);

            gameId.Should().Be(expectedSeasonId);
        }
        [TestMethod]
        public void CallToConstructor_ShouldCreateCorrectly()
        {
            var fakeGameGetter = A.Fake<INhlGameGetter>();
            var fakePlayerGetter = A.Fake<INhlPlayerGetter>();
            var fakeScheduleGetter = A.Fake<INhlScheduleGetter>();

            var cut = new NhlDataGetter(fakeGameGetter, fakePlayerGetter, fakeScheduleGetter); ;

            cut.Should().NotBeNull();
            cut.GameDataGetter.Should().Be(fakeGameGetter);
            cut.PlayerDataGetter.Should().Be(fakePlayerGetter);
            cut.ScheduleDataGetter.Should().Be(fakeScheduleGetter);
        }
    }
}
