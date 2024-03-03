using System.Numerics;
using Entities.DbModels;
using Entities.Models;
using Entities.Types.Mappers;

namespace Services.NhlData.Mappers
{
	public static class MapPlayerStatResponseToPlayer
	{
        /// <summary>
        /// Builds a player stats object
        /// </summary>
        /// <param name="playerStatResponse">Player stat response</param>
        /// <returns>Player stats object</returns>
		public static IPlayerStats BuildPlayerStats(dynamic playerStatResponse)
		{
            if (playerStatResponse.total == 0)
                return new PlayerStats();
            var rawPlayerStats = playerStatResponse.data;
            IPlayerStats playerStats;

            // Goalie stats
            if (rawPlayerStats[0].positionCode == null)
            {
                int goalsAgainst = 0;
                int saves = 0;
                int gamesStarted = 0;
                foreach(var teamStat in rawPlayerStats)
                {
                    goalsAgainst += (int)teamStat.goalsAgainst;
                    saves += (int)teamStat.saves;
                    gamesStarted += (int)teamStat.gamesStarted;
                };

                playerStats = new GoalieStats()
                {
                    goalsAgainst = goalsAgainst,
                    saves = saves,
                    gamesStarted = gamesStarted,
                };

                return playerStats;
            }

            // Skater stats
            int gameCount = 0;
            int faceOffPct = 0;
            int plusMinus = 0;
            int penaltyMinutes = 0;
            int blockedShots = 0;
            int shotsOnGoal = 0;
            int assists = 0;
            int goals = 0;

            foreach (var teamStat in rawPlayerStats)
            {
                gameCount += (int)teamStat.gamesPlayed;
                faceOffPct += teamStat.faceoffWinPct != null ? (int)teamStat.faceoffWinPct : 0;
                plusMinus += (int)teamStat.plusMinus;
                penaltyMinutes += (int)teamStat.penaltyMinutes;
                blockedShots += 0; // New api doesn't have blocked shot data
                shotsOnGoal += (int)teamStat.shots;
                assists += (int)teamStat.assists;
                goals += (int)teamStat.goals;
            };

            playerStats =  new PlayerStats()
            {
                gamesPlayed = gameCount,
                faceoffPercent = faceOffPct / (int)rawPlayerStats.total,
                plusMinus = plusMinus,
                penaltyMinutes = penaltyMinutes,
                blockedShots = blockedShots,
                shotsOnGoal = shotsOnGoal,
                assists = assists,
                goals = goals,
                position = MapPositionStrToPosition.Map(rawPlayerStats[0].positionCode),
            };

            return playerStats;
        }
        /// <summary>
        /// Maps player stats model to a db player
        /// </summary>
        /// <param name="playerStats">The player statistics</param>
        /// <returns>Player object</returns>
        public static DbPlayer MapPlayerStatsToPlayer(IPlayerStats playerStats)
        {
            return new DbPlayer()
            {
                value = playerStats.GetPlayerValue(),
                position = MapPositionToPositionStr.Map(playerStats.position),
            };
        }
    }
}

