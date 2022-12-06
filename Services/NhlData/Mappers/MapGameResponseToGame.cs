using Entities.DbModels;

namespace Services.NhlData.Mappers
{
	public static class MapGameResponseToGame
	{
        /// <summary>
        /// Maps the response from the nhl's api to a game object
        /// </summary>
        /// <param name="message">Response from nhl api</param>
        /// <returns>Game Object</returns>
		public static DbGame Map(dynamic message)
		{
            var homeTeam = message.liveData.boxscore.teams.home.teamStats.teamSkaterStats;
            var awayTeam = message.liveData.boxscore.teams.away.teamStats.teamSkaterStats;

            var game = new DbGame()
            {
                id = (int)message.gamePk,
                homeGoals = (int)homeTeam.goals,
                awayGoals = (int)awayTeam.goals,
                homeTeamId = (int)message.gameData.teams.home.id,
                awayTeamId = (int)message.gameData.teams.away.id,
                homeSOG = (int)homeTeam.shots,
                awaySOG = (int)awayTeam.shots,
                homePPG = (int)homeTeam.powerPlayGoals,
                awayPPG = (int)awayTeam.powerPlayGoals,
                homePIM = (int)homeTeam.pim,
                awayPIM = (int)awayTeam.pim,
                homeFaceOffWinPercent = (double)homeTeam.faceOffWinPercentage,
                awayFaceOffWinPercent = (double)awayTeam.faceOffWinPercentage,
                homeBlockedShots = (int)homeTeam.blocked,
                awayBlockedShots = (int)awayTeam.blocked,
                homeHits = (int)homeTeam.hits,
                awayHits = (int)awayTeam.hits,
                homeTakeaways = (int)homeTeam.takeaways,
                awayTakeaways = (int)awayTeam.takeaways,
                homeGiveaways = (int)homeTeam.giveaways,
                awayGiveaways = (int)awayTeam.giveaways,
                winner = GetWinner((int)homeTeam.goals, (int)awayTeam.goals),
                seasonStartYear = GetSeason((string)message.gameData.game.season),
                gameDate = DateTime.Parse((string)message.gameData.datetime.dateTime),
                hasBeenPlayed = (message.gameData.status.detailedState == "Final") ? true : false,
            };

            if (game == null)
                return new DbGame();

            return game;
        }
        /// <summary>
        /// Determines who won the game.
        /// </summary>
        /// <param name="homeGoals">Home team goals</param>
        /// <param name="awayGoals">Away team goals</param>
        /// <returns>Winner.Home if home won and Winner.Away if away won</returns>
        private static Winner GetWinner(int homeGoals, int awayGoals)
        {
            if (homeGoals > awayGoals)
                return Winner.HOME;
            return Winner.AWAY;
        }
        /// <summary>
        /// Gets the season start year from season string
        /// </summary>
        /// <param name="season">Season string (ex. 20212022)</param>
        /// <returns>Season start year</returns>
        private static int GetSeason(string season)
        {
            var yearStr = season.Substring(0, 4);
            return int.Parse(yearStr);
        }
    }
}

