

using Entities.Models;
using FluentAssertions;
using Microsoft.CSharp.RuntimeBinder;
using Services.NhlData.Mappers;
using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakePlayerStatResponse;

namespace ServicesTests.UnitTests.NhlData.MapperTests
{
    /// <summary>
    /// This test class avoids returning actual values. These are handled in the PlayerStats entities tests.
    /// </summary>
    [TestClass]
    public class MapPlayerStatResponseToPlayerTests
    {
        [TestMethod]
        public void CallToMapPlayerStatsToPlayer_WithGoalie_ShouldReturnValue()
        {
            IPlayerStats playerStats = new GoalieStats()
            {
                gamesStarted = 9,
                saves = 240,
                goalsAgainst = 11,
            };

            var dbPlayer = MapPlayerStatResponseToPlayer.MapPlayerStatsToPlayer(playerStats);
            dbPlayer.value.Should().NotBe(null);
            dbPlayer.value.Should().NotBe(0);
            dbPlayer.position.Should().NotBe(null);
        }
        [TestMethod]
        public void CallToMapPlayerStatsToPlayer_WithDefensivePlayer_ShouldReturnValue()
        {
            IPlayerStats playerStats = new PlayerStats()
            {
                goals = 18,
                assists = 12,
                shotsOnGoal = 60,
                blockedShots = 44,
                penaltyMinutes = 19,
                plusMinus = -4,
                faceoffPercent = .32,
                gamesPlayed = 30,
                position = POSITION.Defenseman,
            };

            var dbPlayer = MapPlayerStatResponseToPlayer.MapPlayerStatsToPlayer(playerStats);
            dbPlayer.value.Should().NotBe(null);
            dbPlayer.value.Should().NotBe(0);
            dbPlayer.position.Should().NotBe(null);
        }
        [TestMethod]
        public void CallToMapPlayerStatsToPlayer_WithCenterPlayer_ShouldReturnValue()
        {
            IPlayerStats playerStats = new PlayerStats()
            {
                goals = 12,
                assists = 6,
                shotsOnGoal = 42,
                blockedShots = 6,
                penaltyMinutes = 10,
                plusMinus = 4,
                faceoffPercent = .7,
                gamesPlayed = 6,
                position = POSITION.Center,
            };

            var dbPlayer = MapPlayerStatResponseToPlayer.MapPlayerStatsToPlayer(playerStats);
            dbPlayer.value.Should().NotBe(null);
            dbPlayer.value.Should().NotBe(0);
            dbPlayer.position.Should().NotBe(null);
        }
        [TestMethod]
        public void CallToMapPlayerStatsToPlayer_WithRightWingPlayer_ShouldReturnValue()
        {
            IPlayerStats playerStats = new PlayerStats()
            {
                goals = 18,
                assists = 12,
                shotsOnGoal = 60,
                blockedShots = 44,
                penaltyMinutes = 19,
                plusMinus = -4,
                faceoffPercent = .32,
                gamesPlayed = 50,
                position = POSITION.RightWing,
            };

            var dbPlayer = MapPlayerStatResponseToPlayer.MapPlayerStatsToPlayer(playerStats);
            dbPlayer.value.Should().NotBe(null);
            dbPlayer.value.Should().NotBe(0);
            dbPlayer.position.Should().NotBe(null);
            dbPlayer.seasonStartYear.Should().NotBe(null);
            dbPlayer.id.Should().NotBe(null);
            dbPlayer.seasonStartYear.Should().NotBe(null);
        }
        [TestMethod]
        public void CallToMapPlayerStatsToPlayer_WithLeftWingPlayer_ShouldReturnValue()
        {
            IPlayerStats playerStats = new PlayerStats()
            {
                goals = 18,
                assists = 12,
                shotsOnGoal = 60,
                blockedShots = 44,
                penaltyMinutes = 19,
                plusMinus = -4,
                faceoffPercent = .32,
                gamesPlayed = 50,
                position = POSITION.LeftWing,
            };

            var dbPlayer = MapPlayerStatResponseToPlayer.MapPlayerStatsToPlayer(playerStats);
            dbPlayer.value.Should().NotBe(null);
            dbPlayer.value.Should().NotBe(0);
            dbPlayer.position.Should().NotBe(null);
        }
        [TestMethod]
        public void CallToBuildPlayerStats_WithEmptyMessage_ShouldThrowError()
        {
            dynamic message = "";
            Action testMap = () => MapPlayerStatResponseToPlayer.BuildPlayerStats(message);

            Assert.ThrowsException<RuntimeBinderException>(testMap);
        }
        [TestMethod]
        public void CallToBuildPlayerStats_WithEmptyStats_ShouldThrowError()
        {
            dynamic message = new FakePlayerStatDataResponse();

            Action testMap = () => MapPlayerStatResponseToPlayer.BuildPlayerStats(message);

            Assert.ThrowsException<ArgumentOutOfRangeException>(testMap);
        }
        [TestMethod]
        public void CallToBuildPlayerStats_WithEmptySplits_ShouldReturnEmptyPlayer()
        {
            dynamic message = new FakePlayerStatDataResponse();
            message.stats = new List<FakeStats>() { new FakeStats() };
            message.stats[0].splits = new List<FakeSplits>();

            PlayerStats dbPlayer = MapPlayerStatResponseToPlayer.BuildPlayerStats(message);

            dbPlayer.id.Should().Be(0);
            dbPlayer.position.Should().Be(POSITION.LeftWing);
        }
        [TestMethod]
        public void CallToBuildPlayerStats_WithGoalieData_ShouldReturnGoalie()
        {
            int goalsAgainst = 3;
            int saves = 30;
            int gamesStarted = 1;

            dynamic message = new FakePlayerStatDataResponse();
            message.stats = new List<FakeStats>() { new FakeStats() };
            message.stats[0].splits[0].stat.faceOffPct = null;
            message.stats[0].splits[0].stat.goalsAgainst = goalsAgainst;
            message.stats[0].splits[0].stat.saves = saves;
            message.stats[0].splits[0].stat.gamesStarted = gamesStarted;

            GoalieStats dbPlayer = MapPlayerStatResponseToPlayer.BuildPlayerStats(message);

            dbPlayer.id.Should().Be(0);
            dbPlayer.position.Should().Be(POSITION.Goalie);
            dbPlayer.goalsAgainst.Should().Be(goalsAgainst);
            dbPlayer.saves.Should().Be(saves);
            dbPlayer.gamesStarted.Should().Be(gamesStarted);
        }
        [TestMethod]
        public void CallToBuildPlayerStats_WithPlayerData_ShouldReturnPlayer()
        {
            double givenFaceOffPct = 60;
            double expectedFaceOffPct = .6;
            int pim = 6;
            int plusMinus = -3;
            int games = 10;
            int blocked = 15;
            int shots = 20;
            int assists = 3;
            int goals = 4;

            dynamic message = new FakePlayerStatDataResponse();
            message.stats = new List<FakeStats>() { new FakeStats() };
            message.stats[0].splits[0].stat.faceOffPct = givenFaceOffPct;
            message.stats[0].splits[0].stat.pim = pim;
            message.stats[0].splits[0].stat.plusMinus = plusMinus;
            message.stats[0].splits[0].stat.games = games;
            message.stats[0].splits[0].stat.blocked = blocked;
            message.stats[0].splits[0].stat.shots = shots;
            message.stats[0].splits[0].stat.assists = assists;
            message.stats[0].splits[0].stat.goals = goals;



            PlayerStats dbPlayer = MapPlayerStatResponseToPlayer.BuildPlayerStats(message);

            dbPlayer.id.Should().Be(0);
            dbPlayer.gamesPlayed.Should().Be(games);
            dbPlayer.blockedShots.Should().Be(blocked);
            dbPlayer.shotsOnGoal.Should().Be(shots);
            dbPlayer.plusMinus.Should().Be(plusMinus);
            dbPlayer.penaltyMinutes.Should().Be(pim);
            dbPlayer.assists.Should().Be(assists);
            dbPlayer.goals.Should().Be(goals);
            dbPlayer.faceoffPercent.Should().Be(expectedFaceOffPct);
        }
    }
}
