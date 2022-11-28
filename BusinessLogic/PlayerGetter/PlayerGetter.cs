using System;
using DataAccess.PlayerRepository;
using Entities.DbModels;
using Entities.Types;
using Microsoft.Extensions.Logging;
using Services.NhlData;

namespace BusinessLogic.PlayerGetter
{
	public class PlayerGetter
	{
        private const int PLAYER_CUTOFF = 300;
        private readonly IPlayerRepository _playerRepo;
        private readonly INhlDataGetter _nhlDataGetter;
        private readonly ILogger<PlayerGetter> _logger;
        public PlayerGetter(IPlayerRepository playerRepo, INhlDataGetter nhlDataGetter, ILoggerFactory loggerFactory)
		{
			_playerRepo = playerRepo;
			_nhlDataGetter = nhlDataGetter;
            _logger = loggerFactory.CreateLogger<PlayerGetter>();
		}
        /// <summary>
        /// Gets all players and their values for a season range and stores to db
        /// </summary>
        /// <param name="seasonYearRange">The years to get player values for</param>
        /// <returns>None</returns>
        public async Task GetPlayers(YearRange seasonYearRange)
        {
            int numberOfPlayersAdded = 0;
            for (int seasonStartYear = seasonYearRange.StartYear; seasonStartYear <= seasonYearRange.EndYear; seasonStartYear++)
            {
                var hasAllplayers = await AllPlayersExist(seasonStartYear);
                var isCurrentSeason = IsCurrentSeason(seasonStartYear, seasonYearRange.EndYear);
                if (hasAllplayers && !isCurrentSeason)
                {
                    _logger.LogInformation("All player data for season " + seasonStartYear.ToString() + " already exists. Skipping...");
                    continue;
                }

                var players = await GetPlayerValues(seasonStartYear);
                await _playerRepo.AddUpdatePlayers(players);
                numberOfPlayersAdded += players.Count();
                _logger.LogInformation("Number of Players Added for Season " + seasonStartYear.ToString() + ": " + players.Count().ToString());
            }
            _logger.LogInformation("Number of Added Players: " + numberOfPlayersAdded.ToString());
        }
        /// <summary>
        /// Gets the players and their values for the given season.
        /// </summary>
        /// <param name="seasonStartYear">Year to get players for</param>
        /// <returns>List of players for the current season and their average value</returns>
        /// <exception cref="NotImplementedException"></exception>
        private async Task<List<DbPlayer>> GetPlayerValues(int seasonStartYear)
        {
            var seasonPlayers = new List<DbPlayer>();
            var teamIds = await _nhlDataGetter.GetTeamsForSeason(seasonStartYear);
            foreach(var teamId in teamIds)
            {
                var playersOnTeam = await GetPlayersOnTeamBySeason(seasonStartYear, teamId);
                seasonPlayers.AddRange(playersOnTeam);
            }

            var distinctPlayers = RemoveDuplicates(seasonPlayers);
            return distinctPlayers;
        }

        /// <summary>
        /// Get players from a team for a given season
        /// </summary>
        /// <param name="teamId">The team id</param>
        /// <returns>A list of players on the team</returns>
        private async Task<List<DbPlayer>> GetPlayersOnTeamBySeason(int seasonStartYear, int teamId)
        {
            var playerIds = await _nhlDataGetter.GetPlayerIdsForTeamBySeason(seasonStartYear, teamId);
            DbPlayer playerValue;
            var playerValues = new List<DbPlayer>();
            foreach(var playerId in playerIds)
            {
                playerValue = await _nhlDataGetter.GetPlayerValueBySeason(playerId, seasonStartYear);
                playerValues.Add(playerValue);
            }

            return playerValues;
        }
        /// <summary>
        /// Removes duplicate players
        /// </summary>
        /// <param name="seasonPlayers">List of players for a season</param>
        /// <returns>List of unique players</returns>
        private List<DbPlayer> RemoveDuplicates(List<DbPlayer> seasonPlayers)
        {
            return seasonPlayers.GroupBy(x => x.id).Select(x => x.First()).ToList();
        }
        /// <summary>
        /// Gets if the season start year is the current season or not
        /// </summary>
        /// <param name="seasonStartYear">Season to check</param>
        /// <param name="currentSeason">The current season start year</param>
        /// <returns>True if the season to check is the same as the current season, otherwise false</returns>
        private bool IsCurrentSeason(int seasonStartYear, int currentSeason)
        {
            return seasonStartYear == currentSeason;
        }

        /// <summary>
        /// Gets if all players are already found
        /// </summary>
        /// <param name="seasonStartYear">Season to check</param>
        /// <returns>True if more players exist than the cutoff, otherwise false</returns>
        private async Task<bool> AllPlayersExist(int seasonStartYear)
        {
            var playerCount = await _playerRepo.GetPlayerCountBySeason(seasonStartYear);
            return playerCount >= PLAYER_CUTOFF;
        }
    }
}

