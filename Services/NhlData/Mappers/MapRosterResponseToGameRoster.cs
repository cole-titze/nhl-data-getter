using System;
using Entities.DbModels;
using Entities.Models;

namespace Services.NhlData.Mappers
{
	public static class MapRosterResponseToGameRoster
	{
        /// <summary>
        /// Maps a roster response to a roster
        /// </summary>
        /// <param name="rosterResponse">Roster response</param>
        /// <returns>List of players mapped to the game</returns>
        public static List<DbGamePlayer> Map(dynamic rosterResponse)
        {
            var roster = new List<DbGamePlayer>();
            if (InvalidTeam(rosterResponse))
                return roster;

            string rawSeason = (string)rosterResponse.gameData.game.season;
            var seasonStartYear = Convert.ToInt32(rawSeason.Substring(0, 4));
            var gameId = Convert.ToInt32(rosterResponse.gameData.game.pk);
            var homeTeamId = (int)rosterResponse.liveData.boxscore.teams.home.team.id;
            var awayTeamId = (int)rosterResponse.liveData.boxscore.teams.away.team.id;

            var homePlayers = rosterResponse.liveData.boxscore.teams.home.skaters;
            var awayPlayers = rosterResponse.liveData.boxscore.teams.away.skaters;
            foreach (var playerId in homePlayers)
            {
                roster.Add(new DbGamePlayer()
                {
                    playerId = (int)playerId,
                    teamId = homeTeamId,
                    gameId = gameId,
                    seasonStartYear = seasonStartYear,
                });
            }
            foreach (var playerId in awayPlayers)
            {
                roster.Add(new DbGamePlayer()
                {
                    playerId = (int)playerId,
                    teamId = awayTeamId,
                    gameId = gameId,
                    seasonStartYear = seasonStartYear,
                });
            }

            var homeGoalies = rosterResponse.liveData.boxscore.teams.home.goalies;
            var awayGoalies = rosterResponse.liveData.boxscore.teams.away.goalies;
            foreach (var playerId in homeGoalies)
            {
                roster.Add(new DbGamePlayer()
                {
                    playerId = (int)playerId,
                    teamId = homeTeamId,
                    gameId = gameId,
                    seasonStartYear = seasonStartYear,
                });
            }
            foreach (var playerId in awayGoalies)
            {
                roster.Add(new DbGamePlayer()
                {
                    playerId = (int)playerId,
                    teamId = awayTeamId,
                    gameId = gameId,
                    seasonStartYear = seasonStartYear,
                });
            }


            return roster;
        }
        /// <summary>
        /// Gets if a roster response is valid or not
        /// </summary>
        /// <param name="message">The roster response</param>
        /// <returns>True if the response is invalid. False if the game is valid</returns>
        private static bool InvalidTeam(dynamic message)
        {
			return message.message == "Object not found";
        }
    }
}
