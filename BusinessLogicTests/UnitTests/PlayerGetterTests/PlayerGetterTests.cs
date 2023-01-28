using BusinessLogic.PlayerGetter;
using DataAccess.PlayerRepository;
using Entities.DbModels;
using Entities.Types;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using Services.NhlData;

namespace BusinessLogicTests.UnitTests.PlayerGetterTests
{
    [TestClass]
    public class PlayerGetterTests
    {
        [TestMethod]
        public async Task CallToGetPlayers_WithBadYearRange_ShouldNotCallRepo()
        {
            var repo = A.Fake<IPlayerRepository>();
            var logger = A.Fake<ILoggerFactory>();
            var nhlDataGetter = A.Fake<NhlDataGetter>();

            var zeroYearRange = new YearRange(2021, 2020);

            var cut = new PlayerGetter(repo, nhlDataGetter, logger);
            await cut.GetPlayers(zeroYearRange);

            A.CallTo(() => repo.AddUpdatePlayers(A<List<DbPlayer>>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => nhlDataGetter.PlayerDataGetter.GetPlayerIdsForTeamBySeason(A<int>.Ignored, A<int>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => nhlDataGetter.PlayerDataGetter.GetPlayerValueBySeason(A<int>.Ignored, A<int>.Ignored)).MustNotHaveHappened();
        }
        [TestMethod]
        public async Task CallToGetPlayers_WithTwoYearRangeAndAllPlayersExist_ShouldCallRepoOnce()
        {
            var repo = A.Fake<IPlayerRepository>();
            var logger = A.Fake<ILoggerFactory>();
            var nhlDataGetter = A.Fake<NhlDataGetter>();

            var twoYearRange = new YearRange(2019, 2020);
            A.CallTo(() => repo.GetPlayerCountBySeason(A<int>.Ignored)).Returns(1000);

            var cut = new PlayerGetter(repo, nhlDataGetter, logger);
            await cut.GetPlayers(twoYearRange);

            A.CallTo(() => repo.AddUpdatePlayers(A<List<DbPlayer>>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => nhlDataGetter.PlayerDataGetter.GetPlayerIdsForTeamBySeason(A<int>.Ignored, A<int>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => nhlDataGetter.PlayerDataGetter.GetPlayerValueBySeason(A<int>.Ignored, A<int>.Ignored)).MustNotHaveHappened();
        }
        [TestMethod]
        public async Task CallToGetPlayers_WithTwoYearRangeAndAllPlayersDontExist_ShouldCallRepoTwice()
        {
            var repo = A.Fake<IPlayerRepository>();
            var logger = A.Fake<ILoggerFactory>();
            var nhlDataGetter = A.Fake<NhlDataGetter>();

            var twoYearRange = new YearRange(2019, 2020);
            A.CallTo(() => repo.GetPlayerCountBySeason(A<int>.Ignored)).Returns(10);

            var cut = new PlayerGetter(repo, nhlDataGetter, logger);
            await cut.GetPlayers(twoYearRange);

            A.CallTo(() => repo.AddUpdatePlayers(A<List<DbPlayer>>.Ignored)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => nhlDataGetter.PlayerDataGetter.GetPlayerIdsForTeamBySeason(A<int>.Ignored, A<int>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => nhlDataGetter.PlayerDataGetter.GetPlayerValueBySeason(A<int>.Ignored, A<int>.Ignored)).MustNotHaveHappened();
        }
        [TestMethod]
        public async Task CallToGetPlayers_WithTwoYearRangeAndTeamsReturned_ShouldCallDataGetter()
        {
            var repo = A.Fake<IPlayerRepository>();
            var logger = A.Fake<ILoggerFactory>();
            var nhlDataGetter = A.Fake<NhlDataGetter>();

            var twoYearRange = new YearRange(2019, 2020);
            A.CallTo(() => repo.GetPlayerCountBySeason(A<int>.Ignored)).Returns(1000);
            A.CallTo(() => nhlDataGetter.PlayerDataGetter.GetPlayerIdsForTeamBySeason(A<int>.Ignored, A<int>.Ignored)).Returns(new List<int>() { 8, 7, 6});
            A.CallTo(() => nhlDataGetter.ScheduleDataGetter.GetTeamsForSeason(A<int>.Ignored)).Returns(new List<int>() { 3, 4, 5, 6});

            var cut = new PlayerGetter(repo, nhlDataGetter, logger);
            await cut.GetPlayers(twoYearRange);

            A.CallTo(() => repo.AddUpdatePlayers(A<List<DbPlayer>>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => nhlDataGetter.PlayerDataGetter.GetPlayerIdsForTeamBySeason(A<int>.Ignored, A<int>.Ignored)).MustHaveHappened(4, Times.Exactly);
            A.CallTo(() => nhlDataGetter.PlayerDataGetter.GetPlayerValueBySeason(A<int>.Ignored, A<int>.Ignored)).MustHaveHappened(12, Times.Exactly);
        }
    }
}
