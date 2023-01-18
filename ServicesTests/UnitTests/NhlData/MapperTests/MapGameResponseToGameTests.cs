using Entities.DbModels;
using FluentAssertions;
using Microsoft.CSharp.RuntimeBinder;
using Services.NhlData.Mappers;
using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse;
using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakeGameDataResponse;

namespace ServicesTests.UnitTests.NhlData.MapperTests
{
    [TestClass]
    public class MapGameResponseToGameTests
    {
        public dynamic ValidResponseFactory()
        {
            dynamic message = new FakeGameResponse();
            message.gameData.game.season = "20212022";
            message.gameData.datetime.dateTime = DateTime.Parse("1/1/2022").ToString();

            return message;
        }
        [TestMethod]
        public void CallToCut_WithEmptyMessage_ShouldThrowError()
        {
            dynamic message = "";

            Action testMap = () => MapGameResponseToGame.Map(message);

            Assert.ThrowsException<RuntimeBinderException>(testMap);
        }
        [TestMethod]
        public void CallToCut_WithPartialResponse_ShouldThrowError()
        {
            dynamic message = new FakeGameResponse()
            {
                liveData = null,
                gameData = new FakeGameData(),
            };

            Action testMap = () => MapGameResponseToGame.Map(message);

            Assert.ThrowsException<RuntimeBinderException>(testMap);
        }
        [TestMethod]
        public void CallToCut_WithEmptySeasonInResponse_ShouldThrowError()
        {
            dynamic message = new FakeGameResponse();
            message.gameData.game.season = "";

            Action testMap = () => MapGameResponseToGame.Map(message);

            Assert.ThrowsException<ArgumentOutOfRangeException>(testMap);
        }
        [TestMethod]
        public void CallToCut_WithInvalidSeason_ShouldThrowError()
        {
            dynamic message = ValidResponseFactory();
            message.gameData.game.season = "adk2022";

            Action testMap = () => MapGameResponseToGame.Map(message);

            Assert.ThrowsException<FormatException>(testMap);
        }
        [TestMethod]
        public void CallToCut_WithValidSeason_ShouldReturnCorrectSeason()
        {
            string givenSeason = "20212022";
            int expectedSeason = 2021;

            dynamic message = ValidResponseFactory();
            message.gameData.game.season = givenSeason;

            DbGame game = MapGameResponseToGame.Map(message);

            game.seasonStartYear.Should().Be(expectedSeason);
        }
        [TestMethod]
        public void CallToCut_WithInvalidDateTimeResponse_ShouldThrowError()
        {
            dynamic message = new FakeGameResponse();
            message.gameData.game.season = "20212022";
            message.gameData.datetime.dateTime = "";

            Action testMap = () => MapGameResponseToGame.Map(message);

            Assert.ThrowsException<FormatException>(testMap);
        }
        [TestMethod]
        public void CallToCut_WithValidResponse_ShouldReturnFilledGame()
        {
            int expectedId = 0;
            dynamic message = ValidResponseFactory();

            DbGame game = MapGameResponseToGame.Map(message);

            game.id.Should().Be(expectedId);
        }
        [TestMethod]
        public void CallToCut_WithId_ShouldReturnCorrectId()
        {
            int expectedId = 7;
            dynamic message = ValidResponseFactory();
            message.gamePk = expectedId;

            DbGame game = MapGameResponseToGame.Map(message);

            game.id.Should().Be(expectedId);
        }
        [TestMethod]
        public void CallToCut_WithGoals_ShouldReturnCorrectGoals()
        {
            int expectedHomeGoals = 7;
            int expectedAwayGoals = 2;
            dynamic message = ValidResponseFactory();
            message.liveData.boxscore.teams.home.teamStats.teamSkaterStats.goals = expectedHomeGoals;
            message.liveData.boxscore.teams.away.teamStats.teamSkaterStats.goals = expectedAwayGoals;

            DbGame game = MapGameResponseToGame.Map(message);

            game.homeGoals.Should().Be(expectedHomeGoals);
            game.awayGoals.Should().Be(expectedAwayGoals);
        }
        [TestMethod]
        public void CallToCut_WithHomeTeamWinning_ShouldReturnCorrectWinner()
        {
            int expectedHomeGoals = 7;
            int expectedAwayGoals = 2;
            dynamic message = ValidResponseFactory();
            message.liveData.boxscore.teams.home.teamStats.teamSkaterStats.goals = expectedHomeGoals;
            message.liveData.boxscore.teams.away.teamStats.teamSkaterStats.goals = expectedAwayGoals;

            DbGame game = MapGameResponseToGame.Map(message);

            game.winner.Should().Be(Winner.HOME);
        }
        [TestMethod]
        public void CallToCut_WithAwayTeamWinning_ShouldReturnCorrectWinner()
        {
            int expectedHomeGoals = 1;
            int expectedAwayGoals = 2;
            dynamic message = ValidResponseFactory();
            message.liveData.boxscore.teams.home.teamStats.teamSkaterStats.goals = expectedHomeGoals;
            message.liveData.boxscore.teams.away.teamStats.teamSkaterStats.goals = expectedAwayGoals;

            DbGame game = MapGameResponseToGame.Map(message);

            game.winner.Should().Be(Winner.AWAY);
        }
        [TestMethod]
        public void CallToCut_WithTeamIds_ShouldReturnCorrectTeamIds()
        {
            int expectedHomeTeamId = 33;
            int expectedAwayTeamId = 6;
            dynamic message = ValidResponseFactory();
            message.gameData.teams.home.id = expectedHomeTeamId;
            message.gameData.teams.away.id = expectedAwayTeamId;

            DbGame game = MapGameResponseToGame.Map(message);

            game.homeTeamId.Should().Be(expectedHomeTeamId);
            game.awayTeamId.Should().Be(expectedAwayTeamId);
        }
        [TestMethod]
        public void CallToCut_WithShotCount_ShouldReturnCorrectShotCount()
        {
            int expectedHomeShots = 35;
            int expectedAwayShots = 27;
            dynamic message = ValidResponseFactory();
            message.liveData.boxscore.teams.home.teamStats.teamSkaterStats.shots = expectedHomeShots;
            message.liveData.boxscore.teams.away.teamStats.teamSkaterStats.shots = expectedAwayShots;

            DbGame game = MapGameResponseToGame.Map(message);

            game.homeSOG.Should().Be(expectedHomeShots);
            game.awaySOG.Should().Be(expectedAwayShots);
        }
        [TestMethod]
        public void CallToCut_WithPPG_ShouldReturnCorrectPPG()
        {
            int expectedHomePPG = 2;
            int expectedAwayPPG = 0;
            dynamic message = ValidResponseFactory();
            message.liveData.boxscore.teams.home.teamStats.teamSkaterStats.powerPlayGoals = expectedHomePPG;
            message.liveData.boxscore.teams.away.teamStats.teamSkaterStats.powerPlayGoals = expectedAwayPPG;

            DbGame game = MapGameResponseToGame.Map(message);

            game.homePPG.Should().Be(expectedHomePPG);
            game.awayPPG.Should().Be(expectedAwayPPG);
        }
        [TestMethod]
        public void CallToCut_WithPIM_ShouldReturnCorrectPIM()
        {
            int expectedHomePIM = 17;
            int expectedAwayPIM = 4;
            dynamic message = ValidResponseFactory();
            message.liveData.boxscore.teams.home.teamStats.teamSkaterStats.pim = expectedHomePIM;
            message.liveData.boxscore.teams.away.teamStats.teamSkaterStats.pim = expectedAwayPIM;

            DbGame game = MapGameResponseToGame.Map(message);

            game.homePIM.Should().Be(expectedHomePIM);
            game.awayPIM.Should().Be(expectedAwayPIM);
        }
        [TestMethod]
        public void CallToCut_WithFaceOffPercents_ShouldReturnCorrectFaceOffPercents()
        {
            double expectedHomeFaceOffWinPercent = .56;
            double expectedAwayFaceOffWinPercent = .44;
            dynamic message = ValidResponseFactory();
            message.liveData.boxscore.teams.home.teamStats.teamSkaterStats.faceOffWinPercentage = expectedHomeFaceOffWinPercent;
            message.liveData.boxscore.teams.away.teamStats.teamSkaterStats.faceOffWinPercentage = expectedAwayFaceOffWinPercent;

            DbGame game = MapGameResponseToGame.Map(message);

            game.homeFaceOffWinPercent.Should().Be(expectedHomeFaceOffWinPercent);
            game.awayFaceOffWinPercent.Should().Be(expectedAwayFaceOffWinPercent);
        }
        [TestMethod]
        public void CallToCut_WithBlockedShots_ShouldReturnCorrectBlockedShots()
        {
            int expectedHomeBlockedShots = 17;
            int expectedAwayBlockedShots = 4;
            dynamic message = ValidResponseFactory();
            message.liveData.boxscore.teams.home.teamStats.teamSkaterStats.blocked = expectedHomeBlockedShots;
            message.liveData.boxscore.teams.away.teamStats.teamSkaterStats.blocked = expectedAwayBlockedShots;

            DbGame game = MapGameResponseToGame.Map(message);

            game.homeBlockedShots.Should().Be(expectedHomeBlockedShots);
            game.awayBlockedShots.Should().Be(expectedAwayBlockedShots);
        }
        [TestMethod]
        public void CallToCut_WithHits_ShouldReturnCorrectHits()
        {
            int expectedHomeHits = 17;
            int expectedAwayHits = 4;
            dynamic message = ValidResponseFactory();
            message.liveData.boxscore.teams.home.teamStats.teamSkaterStats.hits = expectedHomeHits;
            message.liveData.boxscore.teams.away.teamStats.teamSkaterStats.hits = expectedAwayHits;

            DbGame game = MapGameResponseToGame.Map(message);

            game.homeHits.Should().Be(expectedHomeHits);
            game.awayHits.Should().Be(expectedAwayHits);
        }
        [TestMethod]
        public void CallToCut_WithTakeaways_ShouldReturnCorrectTakeaways()
        {
            int expectedHomeTakeaways = 3;
            int expectedAwayTakeaways = 12;
            dynamic message = ValidResponseFactory();
            message.liveData.boxscore.teams.home.teamStats.teamSkaterStats.takeaways = expectedHomeTakeaways;
            message.liveData.boxscore.teams.away.teamStats.teamSkaterStats.takeaways = expectedAwayTakeaways;

            DbGame game = MapGameResponseToGame.Map(message);

            game.homeTakeaways.Should().Be(expectedHomeTakeaways);
            game.awayTakeaways.Should().Be(expectedAwayTakeaways);
        }
        [TestMethod]
        public void CallToCut_WithGiveaways_ShouldReturnCorrectGiveaways()
        {
            int expectedHomeGiveaways = 3;
            int expectedAwayGiveaways = 12;
            dynamic message = ValidResponseFactory();
            message.liveData.boxscore.teams.home.teamStats.teamSkaterStats.giveaways = expectedHomeGiveaways;
            message.liveData.boxscore.teams.away.teamStats.teamSkaterStats.giveaways = expectedAwayGiveaways;

            DbGame game = MapGameResponseToGame.Map(message);

            game.homeGiveaways.Should().Be(expectedHomeGiveaways);
            game.awayGiveaways.Should().Be(expectedAwayGiveaways);
        }
        [TestMethod]
        public void CallToCut_WithFinalState_ShouldReturnHasBeenPlayed()
        {
            bool expectedState = true;
            string givenState = "Final";
            dynamic message = ValidResponseFactory();
            message.gameData.status.detailedState = givenState;

            DbGame game = MapGameResponseToGame.Map(message);

            game.hasBeenPlayed.Should().Be(expectedState);
        }
        [TestMethod]
        public void CallToCut_WithInProgressState_ShouldReturnHasNotBeenPlayed()
        {
            bool expectedState = false;
            string givenState = "In Progress";
            dynamic message = ValidResponseFactory();
            message.gameData.status.detailedState = givenState;

            DbGame game = MapGameResponseToGame.Map(message);

            game.hasBeenPlayed.Should().Be(expectedState);
        }
    }
}
