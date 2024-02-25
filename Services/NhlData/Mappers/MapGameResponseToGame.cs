using Entities.DbModels;
using Newtonsoft.Json.Linq;

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
            var homeTeam = message.homeTeam;
            var awayTeam = message.awayTeam;

            var homePowerPlayConversionStr = (string)homeTeam.powerPlayConversion;
            var awayPowerPlayConversionStr = (string)awayTeam.powerPlayConversion;
            _ = int.TryParse(new string(homePowerPlayConversionStr.TakeWhile(Char.IsDigit).ToArray()), out int homePpg);
            _ = int.TryParse(new string(awayPowerPlayConversionStr.TakeWhile(Char.IsDigit).ToArray()), out int awayPpg);

            // If shootout update goals
            var homeGoals = (int)homeTeam.score;
            var awayGoals = (int)awayTeam.score;
            if (homeGoals == awayGoals)
            {
                homeGoals = message.linescore.totals.home;
                awayGoals = message.linescore.totals.away;
            }


            var game = new DbGame()
            {
                id = (int)message.id,
                homeGoals = homeGoals,
                awayGoals = awayGoals,
                homeTeamId = (int)homeTeam.id,
                awayTeamId = (int)awayTeam.id,
                homeSOG = (int)homeTeam.sog,
                awaySOG = (int)awayTeam.sog,
                homePPG = homePpg,
                awayPPG = awayPpg,
                homePIM = (int)homeTeam.pim,
                awayPIM = (int)awayTeam.pim,
                homeFaceOffWinPercent = (double)homeTeam.faceoffWinningPctg,
                awayFaceOffWinPercent = (double)awayTeam.faceoffWinningPctg,
                homeBlockedShots = (int)homeTeam.blocks,
                awayBlockedShots = (int)awayTeam.blocks,
                homeHits = (int)homeTeam.hits,
                awayHits = (int)awayTeam.hits,
                winner = GetWinner(homeGoals, awayGoals),
                seasonStartYear = GetSeason((string)message.season),
                gameDate = DateTime.Parse((string)message.startTimeUTC),
                hasBeenPlayed = (message.gameState == "OFF") ? true : false,
            };

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
