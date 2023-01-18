using BusinessLogic.GameGetter;
using DataAccess.GameRepository;
using Entities.DbModels;
using Entities.Models;
using Entities.Types;

using FakeItEasy;
using Microsoft.Extensions.Logging;
using Services.NhlData;

namespace BusinessLogicTests.UnitTests.TeamGetterTests;
[TestClass]
public class GameGetterTests
{
    [TestMethod]
    public async Task CallToGetGames_WithBadYearRange_ShouldNotCallRepo()
    {
        var repo = A.Fake<IGameRepository>();
        var logger = A.Fake<ILoggerFactory>();
        var nhlDataGetter = A.Fake<NhlDataGetter>();

        var zeroYearRange = new YearRange(2021, 2020);

        var cut = new GameGetter(repo, nhlDataGetter, logger);
        await cut.GetGames(zeroYearRange);

        A.CallTo(() => repo.GetGameCountInSeason(A<int>.Ignored)).MustNotHaveHappened();
        A.CallTo(() => repo.AddUpdateGames(A<List<DbGame>>.Ignored)).MustNotHaveHappened();
        A.CallTo(() => repo.AddUpdateRosters(A<Dictionary<int,Roster>>.Ignored)).MustNotHaveHappened();
        A.CallTo(() => repo.CacheSeasonOfGames(A<int>.Ignored)).MustNotHaveHappened();
    }
    [TestMethod]
    public async Task CallToGetGames_WithTwoYearRangeWhereGamesExist_ShouldOnlyCallRepoForLastYear()
    {
        var repo = A.Fake<IGameRepository>();
        var logger = A.Fake<ILoggerFactory>();
        var nhlDataGetter = A.Fake<NhlDataGetter>();

        A.CallTo(() => repo.GetGameCountInSeason(A<int>.Ignored)).Returns(200);
        A.CallTo(() => nhlDataGetter.ScheduleDataGetter.GetGameCountInSeason(A<int>.Ignored)).Returns(200);
        var twoYearRange = new YearRange(2020, 2021);

        var cut = new GameGetter(repo, nhlDataGetter, logger);
        await cut.GetGames(twoYearRange);

        A.CallTo(() => repo.GetGameCountInSeason(A<int>.Ignored)).MustHaveHappenedTwiceExactly();
        A.CallTo(() => repo.AddUpdateGames(A<List<DbGame>>.Ignored)).MustHaveHappenedOnceExactly();
        A.CallTo(() => repo.AddUpdateRosters(A<Dictionary<int, Roster>>.Ignored)).MustHaveHappenedOnceExactly();
        A.CallTo(() => repo.CacheSeasonOfGames(A<int>.Ignored)).MustHaveHappenedOnceExactly();
    }
    [TestMethod]
    public async Task CallToGetGames_WithTwoYearRangeWhereGamesHaveNotBeenPlayed_ShouldCallRepoWithAllGames()
    {
        var repo = A.Fake<IGameRepository>();
        var logger = A.Fake<ILoggerFactory>();
        var nhlDataGetter = A.Fake<NhlDataGetter>();

        A.CallTo(() => repo.GetGameCountInSeason(A<int>.Ignored)).Returns(1);
        A.CallTo(() => repo.GetGame(A<int>.Ignored)).Returns(new DbGame() { hasBeenPlayed=false, id=7 });
        A.CallTo(() => repo.AddUpdateGames(A<List<DbGame>>.Ignored)).DoesNothing();
        A.CallTo(() => nhlDataGetter.ScheduleDataGetter.GetGameCountInSeason(A<int>.Ignored)).Returns(1);
        A.CallTo(() => nhlDataGetter.GameDataGetter.GetGame(A<int>.Ignored)).Returns(new DbGame() { id = 7 });

        var twoYearRange = new YearRange(2020, 2021);

        var cut = new GameGetter(repo, nhlDataGetter, logger);
        await cut.GetGames(twoYearRange);

        A.CallTo(() => repo.GetGameCountInSeason(A<int>.Ignored)).MustHaveHappenedTwiceExactly();
        A.CallTo(() => repo.AddUpdateGames(A<List<DbGame>>.Ignored)).MustHaveHappenedOnceExactly();
        A.CallTo(() => repo.AddUpdateGames(A<List<DbGame>>.That.Not.IsEmpty())).MustHaveHappenedOnceExactly();
        A.CallTo(() => repo.AddUpdateRosters(A<Dictionary<int, Roster>>.Ignored)).MustHaveHappenedOnceExactly();
        A.CallTo(() => repo.CacheSeasonOfGames(A<int>.Ignored)).MustHaveHappenedOnceExactly();
    }
    [TestMethod]
    public async Task CallToGetGames_WithTwoYearRangeWhereGamesHaveBeenPlayed_ShouldNotHaveNewGamesToUpdate()
    {
        var repo = A.Fake<IGameRepository>();
        var logger = A.Fake<ILoggerFactory>();
        var nhlDataGetter = A.Fake<NhlDataGetter>();

        A.CallTo(() => repo.GetGameCountInSeason(A<int>.Ignored)).Returns(100);
        A.CallTo(() => repo.GetGame(A<int>.Ignored)).Returns(new DbGame() { hasBeenPlayed = true, id = 7 });
        A.CallTo(() => repo.AddUpdateGames(A<List<DbGame>>.Ignored)).DoesNothing();
        A.CallTo(() => nhlDataGetter.ScheduleDataGetter.GetGameCountInSeason(A<int>.Ignored)).Returns(100);
        A.CallTo(() => nhlDataGetter.GameDataGetter.GetGame(A<int>.Ignored)).Returns(new DbGame() { id = 7 });

        var twoYearRange = new YearRange(2020, 2021);

        var cut = new GameGetter(repo, nhlDataGetter, logger);
        await cut.GetGames(twoYearRange);

        A.CallTo(() => repo.GetGameCountInSeason(A<int>.Ignored)).MustHaveHappenedTwiceExactly();
        A.CallTo(() => repo.AddUpdateGames(A<List<DbGame>>.Ignored)).MustHaveHappenedOnceExactly();
        A.CallTo(() => repo.AddUpdateGames(A<List<DbGame>>.That.Not.IsEmpty())).MustNotHaveHappened();
        A.CallTo(() => repo.AddUpdateRosters(A<Dictionary<int, Roster>>.Ignored)).MustHaveHappenedOnceExactly();
        A.CallTo(() => repo.CacheSeasonOfGames(A<int>.Ignored)).MustHaveHappenedOnceExactly();
    }
    [TestMethod]
    public async Task CallToGetGames_WithTwoYearRangeWhereGamesNeverFound_ShouldHaveNewGamesToUpdate()
    {
        var repo = A.Fake<IGameRepository>();
        var logger = A.Fake<ILoggerFactory>();
        var nhlDataGetter = A.Fake<NhlDataGetter>();

        A.CallTo(() => repo.GetGameCountInSeason(A<int>.Ignored)).Returns(0);
        A.CallTo(() => repo.GetGame(A<int>.Ignored)).Returns(new DbGame() { hasBeenPlayed = true, id = 7 });
        A.CallTo(() => repo.AddUpdateGames(A<List<DbGame>>.Ignored)).DoesNothing();
        A.CallTo(() => nhlDataGetter.ScheduleDataGetter.GetGameCountInSeason(A<int>.Ignored)).Returns(1);
        A.CallTo(() => nhlDataGetter.GameDataGetter.GetGame(A<int>.Ignored)).Returns(new DbGame() { id = 7 });

        var twoYearRange = new YearRange(2020, 2021);

        var cut = new GameGetter(repo, nhlDataGetter, logger);
        await cut.GetGames(twoYearRange);

        A.CallTo(() => repo.GetGameCountInSeason(A<int>.Ignored)).MustHaveHappenedTwiceExactly();
        A.CallTo(() => repo.AddUpdateGames(A<List<DbGame>>.Ignored)).MustHaveHappenedTwiceExactly();
    }
}
