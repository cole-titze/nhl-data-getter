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
            message.season = "20212022";
            message.startTimeUTC = DateTime.Parse("1/1/2022").ToString();

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
                homeTeam = null
            };

            Action testMap = () => MapGameResponseToGame.Map(message);

            Assert.ThrowsException<RuntimeBinderException>(testMap);
        }
        [TestMethod]
        public void CallToCut_WithEmptySeasonInResponse_ShouldThrowError()
        {
            dynamic message = new FakeGameResponse();
            message.season = "";

            Action testMap = () => MapGameResponseToGame.Map(message);

            Assert.ThrowsException<ArgumentOutOfRangeException>(testMap);
        }
        [TestMethod]
        public void CallToCut_WithInvalidSeason_ShouldThrowError()
        {
            dynamic message = ValidResponseFactory();
            message.season = "adk2022";

            Action testMap = () => MapGameResponseToGame.Map(message);

            Assert.ThrowsException<FormatException>(testMap);
        }
        [TestMethod]
        public void CallToCut_WithValidSeason_ShouldReturnCorrectSeason()
        {
            string givenSeason = "20212022";
            int expectedSeason = 2021;

            dynamic message = ValidResponseFactory();
            message.season = givenSeason;

            DbGame game = MapGameResponseToGame.Map(message);

            game.seasonStartYear.Should().Be(expectedSeason);
        }
        [TestMethod]
        public void CallToCut_WithInvalidDateTimeResponse_ShouldThrowError()
        {
            dynamic message = new FakeGameResponse();
            message.season = "20212022";
            message.startTimeUTC = "";

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
            message.id = expectedId;

            DbGame game = MapGameResponseToGame.Map(message);

            game.id.Should().Be(expectedId);
        }
        [TestMethod]
        public void CallToCut_WithGoals_ShouldReturnCorrectGoals()
        {
            int expectedHomeGoals = 7;
            int expectedAwayGoals = 2;
            dynamic message = ValidResponseFactory();
            message.homeTeam.score = expectedHomeGoals;
            message.awayTeam.score = expectedAwayGoals;

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
            message.homeTeam.score = expectedHomeGoals;
            message.awayTeam.score = expectedAwayGoals;

            DbGame game = MapGameResponseToGame.Map(message);

            game.winner.Should().Be(Winner.HOME);
        }
        [TestMethod]
        public void CallToCut_WithAwayTeamWinning_ShouldReturnCorrectWinner()
        {
            int expectedHomeGoals = 1;
            int expectedAwayGoals = 2;
            dynamic message = ValidResponseFactory();
            message.homeTeam.score = expectedHomeGoals;
            message.awayTeam.score = expectedAwayGoals;

            DbGame game = MapGameResponseToGame.Map(message);

            game.winner.Should().Be(Winner.AWAY);
        }
        [TestMethod]
        public void CallToCut_WithTeamIds_ShouldReturnCorrectTeamIds()
        {
            int expectedHomeTeamId = 33;
            int expectedAwayTeamId = 6;
            dynamic message = ValidResponseFactory();
            message.homeTeam.id = expectedHomeTeamId;
            message.awayTeam.id = expectedAwayTeamId;

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
            message.homeTeam.sog = expectedHomeShots;
            message.awayTeam.sog = expectedAwayShots;

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
            message.homeTeam.powerPlayConversion = "2/4";
            message.awayTeam.powerPlayConversion = "0/0";

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
            message.homeTeam.pim = expectedHomePIM;
            message.awayTeam.pim = expectedAwayPIM;

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
            message.homeTeam.faceoffWinningPctg = expectedHomeFaceOffWinPercent;
            message.awayTeam.faceoffWinningPctg = expectedAwayFaceOffWinPercent;

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
            message.homeTeam.blocks = expectedHomeBlockedShots;
            message.awayTeam.blocks = expectedAwayBlockedShots;

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
            message.homeTeam.hits = expectedHomeHits;
            message.awayTeam.hits = expectedAwayHits;

            DbGame game = MapGameResponseToGame.Map(message);

            game.homeHits.Should().Be(expectedHomeHits);
            game.awayHits.Should().Be(expectedAwayHits);
        }
        [TestMethod]
        public void CallToCut_WithFinalState_ShouldReturnHasBeenPlayed()
        {
            bool expectedState = true;
            string givenState = "OFF";
            dynamic message = ValidResponseFactory();
            message.gameState = givenState;

            DbGame game = MapGameResponseToGame.Map(message);

            game.hasBeenPlayed.Should().Be(expectedState);
        }
        [TestMethod]
        public void CallToCut_WithInProgressState_ShouldReturnHasNotBeenPlayed()
        {
            bool expectedState = false;
            string givenState = "LIVE";
            dynamic message = ValidResponseFactory();
            message.gameState = givenState;

            DbGame game = MapGameResponseToGame.Map(message);

            game.hasBeenPlayed.Should().Be(expectedState);
        }
    }
}
