using System.Numerics;
using Entities.DbModels;
using Entities.Models;

namespace Services.NhlData.Mappers
{
    public static class MapRosterResponseToGameRoster
	{
        /// <summary>
        /// Maps the teams roster into GamePlayer objects
        /// </summary>
        /// <param name="rosterResponse">A response for an nhl team that holds a roster</param>
        /// <param name="game">The game to build the players for</param>
        /// <param name="teamId">The team the players belong to</param>
        /// <returns>List of players in the game</returns>
        public static List<DbGamePlayer> MapTeamRoster(dynamic rosterResponse, DbGame game, int teamId)
        {
            var roster = new List<DbGamePlayer>();

            foreach(var player in rosterResponse.forwards)
            {
                roster = AddPlayer(roster, player, game, teamId);
            }

            foreach (var player in rosterResponse.defensemen)
            {
                roster = AddPlayer(roster, player, game, teamId);
            }

            foreach (var player in rosterResponse.goalies)
            {
                roster = AddPlayer(roster, player, game, teamId);
            }

            return roster;
        }

        private static List<DbGamePlayer> AddPlayer(List<DbGamePlayer> roster, dynamic player, DbGame game, int teamId)
        {
            roster.Add(new DbGamePlayer()
            {
                gameId = game.id,
                teamId = teamId,
                playerId = player.person.id,
                seasonStartYear = game.seasonStartYear,
            });

            return roster;
        }

        /// <summary>
        /// Maps a roster response to a roster
        /// </summary>
        /// <param name="rosterResponse">Roster response</param>
        /// <returns>List of players mapped to the game</returns>
        public static Roster MapPlayedGame(dynamic rosterResponse)
        {
            var roster = new Roster();

            var seasonStartYear = (int)rosterResponse.season;
            var gameId = (int)rosterResponse.id;
            var homeTeamId = (int)rosterResponse.homeTeam.id;
            var awayTeamId = (int)rosterResponse.awayTeam.id;

            // Forwards
            var homeForwards = rosterResponse.playerByGameStats.homeTeam.forwards;
            var awayForwards = rosterResponse.playerByGameStats.awayTeam.forwards;
            foreach (var player in homeForwards)
            {
                int playerId = (int)player.playerId;
                // Remove duplicate players
                if (roster.homeTeam.Where(i => i.gameId == gameId && i.playerId == playerId).Any())
                    continue;

                roster.homeTeam.Add(new DbGamePlayer()
                {
                    playerId = playerId,
                    teamId = homeTeamId,
                    gameId = gameId,
                    seasonStartYear = seasonStartYear,
                });
            }
            foreach (var player in awayForwards)
            {
                int playerId = (int)player.playerId;
                // Remove duplicate players
                if (roster.awayTeam.Where(i => i.gameId == gameId && i.playerId == playerId).Any())
                    continue;

                roster.awayTeam.Add(new DbGamePlayer()
                {
                    playerId = playerId,
                    teamId = awayTeamId,
                    gameId = gameId,
                    seasonStartYear = seasonStartYear,
                });
            }

            // Defenseman
            var homeDefenseman = rosterResponse.playerByGameStats.homeTeam.defense;
            var awayDefenseman = rosterResponse.playerByGameStats.awayTeam.defense;
            foreach (var player in homeDefenseman)
            {
                var playerId = (int)player.playerId;
                // Remove duplicate players
                if (roster.homeTeam.Where(i => i.gameId == gameId && i.playerId == playerId).Any())
                    continue;

                roster.homeTeam.Add(new DbGamePlayer()
                {
                    playerId = playerId,
                    teamId = homeTeamId,
                    gameId = gameId,
                    seasonStartYear = seasonStartYear,
                });
            }
            foreach (var player in awayDefenseman)
            {
                var playerId = (int)player.playerId;
                // Remove duplicate players
                if (roster.awayTeam.Where(i => i.gameId == gameId && i.playerId == playerId).Any())
                    continue;

                roster.awayTeam.Add(new DbGamePlayer()
                {
                    playerId = player.playerId,
                    teamId = awayTeamId,
                    gameId = gameId,
                    seasonStartYear = seasonStartYear,
                });
            }

            // Goalies
            var homeGoalies = rosterResponse.playerByGameStats.awayTeam.goalies;
            var awayGoalies = rosterResponse.playerByGameStats.awayTeam.goalies;
            foreach (var player in homeGoalies)
            {
                roster.homeTeam.Add(new DbGamePlayer()
                {
                    playerId = (int)player.playerId,
                    teamId = homeTeamId,
                    gameId = gameId,
                    seasonStartYear = seasonStartYear,
                });
            }
            foreach (var player in awayGoalies)
            {
                roster.awayTeam.Add(new DbGamePlayer()
                {
                    playerId = (int)player.playerId,
                    teamId = awayTeamId,
                    gameId = gameId,
                    seasonStartYear = seasonStartYear,
                });
            }


            return roster;
        }
    }
}
