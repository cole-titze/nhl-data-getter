using Entities.Models;
using FluentAssertions;

namespace EntitiesTests.UnitTests.ModelTests
{
    [TestClass]
    public class IPlayerStatsTests
    {
        [TestMethod]
        public void CallToGetPlayerValue_WithGoalie_ShouldHaveCorrectValue()
        {
            double correctValue = 105;
            IPlayerStats playerStats = new GoalieStats()
            {
                gamesStarted = 9,
                saves = 240,
                goalsAgainst = 11,
            };

            var playerValue = playerStats.GetPlayerValue();
            var roundedValue = Math.Round(playerValue, 2);
            roundedValue.Should().Be(correctValue);
        }
        [TestMethod]
        public void CallToGetPlayerValue_WithEmptyPlayer_ShouldHaveZeroValue()
        {
            IPlayerStats playerStats = new PlayerStats();

            var playerValue = playerStats.GetPlayerValue();
            playerValue.Should().Be(0);
        }
        [TestMethod]
        public void CallToGetPlayerValue_WithLowGamesPlayed_ShouldHaveZeroValue()
        {
            IPlayerStats playerStats = new PlayerStats()
            {
                gamesPlayed = 3,
            };

            var playerValue = playerStats.GetPlayerValue();
            playerValue.Should().Be(0);
        }
        [TestMethod]
        public void CallToGetPlayerValue_WithCenterPlayer_ShouldHaveCorrectValue()
        {
            double correctValue = 167.94;
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

            var playerValue = playerStats.GetPlayerValue();
            var roundedValue = Math.Round(playerValue, 2);
            roundedValue.Should().Be(correctValue);
        }
        [TestMethod]
        public void CallToGetPlayerValue_WithRightWingPlayer_ShouldHaveCorrectValue()
        {
            double correctValue = 30.88;
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

            var playerValue = playerStats.GetPlayerValue();
            var roundedValue = Math.Round(playerValue, 2);
            roundedValue.Should().Be(correctValue);
        }
        [TestMethod]
        public void CallToGetPlayerValue_WithLeftWingPlayer_ShouldHaveCorrectValue()
        {
            double correctValue = 30.88;
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

            var playerValue = playerStats.GetPlayerValue();
            var roundedValue = Math.Round(playerValue, 2);
            roundedValue.Should().Be(correctValue);
        }
        [TestMethod]
        public void CallToGetPlayerValue_WithDefensivePlayer_ShouldHaveCorrectValue()
        {
            double correctValue = 51.47;
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

            var playerValue = playerStats.GetPlayerValue();
            var roundedValue = Math.Round(playerValue, 2);
            roundedValue.Should().Be(correctValue);
        }
        [TestMethod]
        public void CallToGetPlayerValue_WithGoalieSmallGamesPlayed_ShouldHaveZeroValue()
        {
            double correctValue = 0;
            IPlayerStats playerStats = new GoalieStats()
            {
                gamesStarted = 2,
                saves = 40,
                goalsAgainst = 2,
            };

            var playerValue = playerStats.GetPlayerValue();
            var roundedValue = Math.Round(playerValue, 2);
            roundedValue.Should().Be(correctValue);
        }
    }
}