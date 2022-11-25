namespace Entities.DbModels
{
    public enum Winner
    {
        HOME,
        AWAY
    }
    public class DbGame
    {
        public int id { get; set; } = -1;
        public int homeTeamId { get; set; }
        public int awayTeamId { get; set; }
        public int seasonStartYear { get; set; }
        public DateTime gameDate { get; set; }
        public int homeGoals { get; set; }
        public int awayGoals { get; set; }
        public int winner { get; set; }
        public int homeSOG { get; set; }
        public int awaySOG { get; set; }
        public int homePPG { get; set; }
        public int awayPPG { get; set; }
        public int homePIM { get; set; }
        public int awayPIM { get; set; }
        public double homeFaceOffWinPercent { get; set; }
        public double awayFaceOffWinPercent { get; set; }
        public int homeBlockedShots { get; set; }
        public int awayBlockedShots { get; set; }
        public int homeHits { get; set; }
        public int awayHits { get; set; }
        public int homeTakeaways { get; set; }
        public int awayTakeaways { get; set; }
        public int homeGiveaways { get; set; }
        public int awayGiveaways { get; set; }
        public bool hasBeenPlayed { get; set; }

        public void Clone(DbGame game)
        {
            id = game.id;
            homeTeamId = game.homeTeamId;
            awayTeamId = game.awayTeamId;
            seasonStartYear = game.seasonStartYear;
            gameDate = game.gameDate;
            homeGoals = game.homeGoals;
            awayGoals = game.awayGoals;
            winner = game.winner;
            homeSOG = game.homeSOG;
            awaySOG = game.awaySOG;
            homePPG = game.homePPG;
            awayPPG = game.awayPPG;
            homePIM = game.homePIM;
            awayPIM = game.awayPIM;
            homeFaceOffWinPercent = game.homeFaceOffWinPercent;
            awayFaceOffWinPercent = game.awayFaceOffWinPercent;
            homeBlockedShots = game.homeBlockedShots;
            awayBlockedShots = game.awayBlockedShots;
            homeHits = game.homeHits;
            awayHits = game.awayHits;
            homeTakeaways = game.homeTakeaways;
            awayTakeaways = game.awayTakeaways;
            homeGiveaways = game.homeGiveaways;
            awayGiveaways = game.awayGiveaways;
            hasBeenPlayed = game.hasBeenPlayed;
        }
        /// <summary>
        /// Gets whether a game is valid or not
        /// </summary>
        /// <returns>True if both teams won 0 faceoffs</returns>
        public bool IsValid()
        {
            return id != -1;
        }
    }
}