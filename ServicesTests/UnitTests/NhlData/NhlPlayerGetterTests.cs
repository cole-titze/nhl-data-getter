using FakeItEasy;
using Services.NhlData;
using ServicesTests.UnitTests.NhlData.Fakes;
using Microsoft.Extensions.Logging;
using Entities.DbModels;
using FluentAssertions;
using Entities.Models;

namespace ServicesTests.UnitTests.NhlData
{
    [TestClass]
    public class NhlPlayerGetterTests
    {

        [TestMethod]
        public void CallToGetGameRoster_WithNullGameResponse_ShouldReturnEmptyRoster()
        {
            var fakeRequestMaker = new FakePlayerRequestMaker();
            var game = new DbGame();
            game.id = FakePlayerRequestMaker.invalidGameId;

            var cut = new NhlPlayerGetter(fakeRequestMaker, A.Fake<ILoggerFactory>());
            Roster roster = cut.GetGameRoster(game).Result;

            roster.homeTeam.Count.Should().Be(0);
            roster.awayTeam.Count.Should().Be(0);
        }
        [TestMethod]
        public void CallToGetGameRoster_WithFilledGameResponse_ShouldReturnFilledRoster()
        {
            var fakeRequestMaker = new FakePlayerRequestMaker();
            var game = new DbGame();
            game.id = FakePlayerRequestMaker.invalidGameId;
            game.homeTeamId = 8;
            game.awayTeamId = 9;

            var cut = new NhlPlayerGetter(fakeRequestMaker, A.Fake<ILoggerFactory>());
            Roster roster = cut.GetGameRoster(game).Result;

            fakeRequestMaker.teamCallCount.Should().Be(2);
            roster.homeTeam.Count.Should().Be(15);
            roster.awayTeam.Count.Should().Be(15);
        }
        [TestMethod]
        public void CallToGetGameRoster_WithTwoCallsToSameTeam_ShouldUseCache()
        {
            var fakeRequestMaker = new FakePlayerRequestMaker();
            var game = new DbGame();
            game.id = FakePlayerRequestMaker.invalidGameId;
            game.awayTeamId = 8;
            game.homeTeamId = 8;

            var cut = new NhlPlayerGetter(fakeRequestMaker, A.Fake<ILoggerFactory>());
            Roster roster = cut.GetGameRoster(game).Result;
            
            fakeRequestMaker.teamCallCount.Should().Be(1);
            roster.homeTeam.Count.Should().Be(15);
            roster.awayTeam.Count.Should().Be(15);
        }
        [TestMethod]
        public void CallToGetGameRoster_WithValidGameResponse_ShouldFillRoster()
        {
            var fakeRequestMaker = new FakePlayerRequestMaker();
            var game = new DbGame();
            game.id = FakePlayerRequestMaker.validGameId;
            game.awayTeamId = 10;
            game.homeTeamId = 17;

            var cut = new NhlPlayerGetter(fakeRequestMaker, A.Fake<ILoggerFactory>());
            Roster roster = cut.GetGameRoster(game).Result;

            fakeRequestMaker.teamCallCount.Should().Be(0);
            roster.homeTeam.Count.Should().Be(26);
            roster.awayTeam.Count.Should().Be(7);
        }
        [TestMethod]
        public void CallToGetPlayerIds_WithInvalidPlayerResponse_ShouldReturnEmptyIdList()
        {
            var fakeRequestMaker = new FakePlayerRequestMaker();
            int seasonStartYear = 2022;
            int teamId = FakePlayerRequestMaker.invalidTeamId;

            var cut = new NhlPlayerGetter(fakeRequestMaker, A.Fake<ILoggerFactory>());
            var teamRoster = cut.GetPlayerIdsForTeamBySeason(seasonStartYear, teamId).Result;

            teamRoster.Count.Should().Be(0);
        }
        [TestMethod]
        public void CallToGetPlayerIds_WithValidPlayerResponse_ShouldReturnCorrectIdList()
        {
            var fakeRequestMaker = new FakePlayerRequestMaker();
            int seasonStartYear = 2021;
            int teamId = FakePlayerRequestMaker.validTeamId;

            var cut = new NhlPlayerGetter(fakeRequestMaker, A.Fake<ILoggerFactory>());
            var teamRoster = cut.GetPlayerIdsForTeamBySeason(seasonStartYear, teamId).Result;

            teamRoster.Count.Should().Be(2);
            teamRoster[0].Should().Be(7);
            teamRoster[1].Should().Be(3);
        }
        [TestMethod]
        public void CallToGetPlayerValueBySeason_WithInvalidPlayerResponse_ShouldReturnEmptyPlayer()
        {
            var fakeRequestMaker = new FakePlayerRequestMaker();
            int seasonStartYear = 2021;
            int playerId = FakePlayerRequestMaker.invalidPeopleId;
            int expectedInvalidPlayerId = 0;

            var cut = new NhlPlayerGetter(fakeRequestMaker, A.Fake<ILoggerFactory>());
            var player = cut.GetPlayerValueBySeason(playerId, seasonStartYear).Result;

            player.id.Should().Be(expectedInvalidPlayerId);
        }
        [TestMethod]
        public void CallToGetPlayerValueBySeason_WithInvalidPositionResponse_ShouldReturnEmptyPlayer()
        {
            var fakeRequestMaker = new FakePlayerRequestMaker();
            int seasonStartYear = 2021;
            int playerId = FakePlayerRequestMaker.invalidPositionId;
            int expectedInvalidPlayerId = 0;

            var cut = new NhlPlayerGetter(fakeRequestMaker, A.Fake<ILoggerFactory>());
            var player = cut.GetPlayerValueBySeason(playerId, seasonStartYear).Result;

            player.id.Should().Be(expectedInvalidPlayerId);
        }
        [TestMethod]
        public void CallToGetPlayerValueBySeason_WithValidResponses_ShouldReturnFilledPlayer()
        {
            var fakeRequestMaker = new FakePlayerRequestMaker();
            int seasonStartYear = 2021;
            int expectedPlayerId = FakePlayerRequestMaker.validPeopleId;

            var cut = new NhlPlayerGetter(fakeRequestMaker, A.Fake<ILoggerFactory>());
            var player = cut.GetPlayerValueBySeason(expectedPlayerId, seasonStartYear).Result;

            player.id.Should().Be(expectedPlayerId);
            player.seasonStartYear.Should().Be(seasonStartYear);
        }
    }
}
