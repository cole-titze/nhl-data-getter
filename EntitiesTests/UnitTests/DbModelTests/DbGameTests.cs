using Entities.DbModels;
using FluentAssertions;

namespace EntitiesTests.UnitTests.DbModelTests
{
    [TestClass]
    public class DbGameTests
    {
        private void CheckClonedGame(DbGame cut, DbGame copy)
        {
            copy.id.Should().Be(cut.id);
            copy.homeTeamId.Should().Be(cut.homeTeamId);
            copy.awayTeamId.Should().Be(cut.awayTeamId);
            copy.seasonStartYear.Should().Be(cut.seasonStartYear);
            copy.gameDate.Should().Be(cut.gameDate);
            copy.homeGoals.Should().Be(cut.homeGoals);
            copy.awayGoals.Should().Be(cut.awayGoals);
            copy.winner.Should().Be(cut.winner);
            copy.homeSOG.Should().Be(cut.homeSOG);
            copy.awaySOG.Should().Be(cut.awaySOG);
            copy.homePPG.Should().Be(cut.homePPG);
            copy.awayPPG.Should().Be(cut.awayPPG);
            copy.homePIM.Should().Be(cut.homePIM);
            copy.awayPIM.Should().Be(cut.awayPIM);
            copy.homeFaceOffWinPercent.Should().Be(cut.homeFaceOffWinPercent);
            copy.awayFaceOffWinPercent.Should().Be(cut.awayFaceOffWinPercent);
            copy.homeBlockedShots.Should().Be(cut.homeBlockedShots);
            copy.awayBlockedShots.Should().Be(cut.awayBlockedShots);
            copy.homeHits.Should().Be(cut.homeHits);
            copy.awayHits.Should().Be(cut.awayHits);
            copy.homeTakeaways.Should().Be(cut.homeTakeaways);
            copy.awayTakeaways.Should().Be(cut.awayTakeaways);
            copy.homeGiveaways.Should().Be(cut.homeGiveaways);
            copy.awayGiveaways.Should().Be(cut.awayGiveaways);
            copy.hasBeenPlayed.Should().Be(cut.hasBeenPlayed);
        }
        [TestMethod]
        public void CallToIsValid_WithInvalidGame_ShouldReturnFalse()
        {
            bool expectedIsValid = false;

            var cut = new DbGame();
            var isValid = cut.IsValid();

            isValid.Should().Be(expectedIsValid);
        }
        [TestMethod]
        public void CallToIsValid_WithValidGame_ShouldReturnTrue()
        {
            bool expectedIsValid = true;

            var cut = new DbGame();
            cut.id = 7;

            var isValid = cut.IsValid();

            isValid.Should().Be(expectedIsValid);
        }
        [TestMethod]
        public void CallToClone_WithEmptyGame_ShouldCopyEmptyGame()
        {
            var cut = new DbGame();

            DbGame copy = new DbGame();
            copy.Clone(cut);

            CheckClonedGame(cut, copy);
        }
        [TestMethod]
        public void CallToClone_WithGame_ShouldCreateCopyGame()
        {
            var cut = new DbGame()
            {
                id = 8,
                homeTeamId = 4,
                awayTeamId = 23,
                seasonStartYear = 2021,
                gameDate = DateTime.Parse("02/27/2022"),
                homeGoals = 8,
                awayGoals = 3,
                winner = Winner.HOME,
                homeSOG = 33,
                awaySOG = 22,
                homePPG = 2,
                awayPPG = 1,
                homePIM = 2,
                awayPIM = 8,
                homeFaceOffWinPercent = .6,
                awayFaceOffWinPercent = .4,
                homeBlockedShots = 5,
                awayBlockedShots = 6,
                homeHits = 12,
                awayHits = 6,
                homeTakeaways = 12,
                awayTakeaways = 5,
                homeGiveaways = 5,
                awayGiveaways = 12,
                hasBeenPlayed = true,
            };

            DbGame copy = new DbGame();
            copy.Clone(cut);

            CheckClonedGame(cut, copy);
        }
    }
}
