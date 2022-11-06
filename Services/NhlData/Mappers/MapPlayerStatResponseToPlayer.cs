using Entities.DbModels;
using Entities.Models;

namespace Services.NhlData.Mappers
{
	public static class MapPlayerStatResponseToPlayer
	{
        /// <summary>
        /// Maps a player statistic response to a player
        /// </summary>
        /// <param name="playerStatResponse">Player stat response</param>
        /// <returns>Player object</returns>
		public static DbPlayer Map(dynamic playerStatResponse)
		{
            if (playerStatResponse.stats[0].splits.Count == 0)
                return new DbPlayer();
            var rawPlayer = playerStatResponse.stats[0].splits[0].stat;
            IPlayerStats playerStats;

            if (rawPlayer.faceOffPct == null)
            {
                playerStats = new GoalieStats()
                {
                    goalsAgainst = rawPlayer.goalsAgainst,
                    saves = rawPlayer.saves,
                    gamesStarted = rawPlayer.gamesStarted,
                };
                return MapPlayerStatsToPlayer(playerStats);
            }

            playerStats =  new PlayerStats()
            {
                gamesPlayed = rawPlayer.games,
                faceoffPercent = (rawPlayer.faceOffPct / 100),
                plusMinus = rawPlayer.plusMinus,
                penaltyMinutes = rawPlayer.pim,
                blockedShots = rawPlayer.blocked,
                shotsOnGoal = rawPlayer.shots,
                assists = rawPlayer.assists,
                goals = rawPlayer.goals,
            };

            return MapPlayerStatsToPlayer(playerStats);
        }
        /// <summary>
        /// Maps player stats model to a db player
        /// </summary>
        /// <param name="playerStats">The player statistics</param>
        /// <returns>Player object</returns>
        private static DbPlayer MapPlayerStatsToPlayer(IPlayerStats playerStats)
        {
            return new DbPlayer()
            {
                value = playerStats.GetPlayerValue(),
            };
        }
    }
}

