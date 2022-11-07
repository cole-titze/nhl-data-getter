using DataAccess.GameRepository;
using Entities.DbModels;
using Entities.Types;
using Microsoft.Extensions.Logging;
using Services.NhlData;

namespace BusinessLogic.GameGetter
{
	public class GameGetter
	{
        private IGameRepository _gameRepo;
        private INhlDataGetter _nhlDataGetter;
        private readonly ILogger _logger;
        public GameGetter(IGameRepository gameRepository, INhlDataGetter nhlDataGetter, ILogger logger)
        {
            _gameRepo = gameRepository;
            _nhlDataGetter = nhlDataGetter;
            _logger = logger;
        }
        /// <summary>
        /// Gets all nhl games within the season range. If the game is already in the database, it is skipped.
        /// </summary>
        public async Task GetGames(YearRange seasonYearRange)
        {
            int numberOfGamesAdded;
            for (int seasonStartYear = seasonYearRange.StartYear; seasonStartYear <= seasonYearRange.EndYear; seasonStartYear++)
            {
                numberOfGamesAdded = 0;
                if (await SeasonGamesExist(seasonStartYear))
                {
                    _logger.LogInformation("All game data for season " + seasonStartYear.ToString() + " already exists. Skipping...");
                    continue;
                }

                var seasonGameCount = await _nhlDataGetter.GetGameCountInSeason(seasonStartYear);
                var seasonGames = await GetSeasonGames(seasonStartYear, seasonGameCount);
                await _gameRepo.AddGames(seasonGames);
                _logger.LogInformation("Number of Games Added To Season" + seasonStartYear.ToString() + ": " + numberOfGamesAdded.ToString());
            }
        }
        /// <summary>
        /// Gets if all of a seasons games are already found
        /// </summary>
        /// <param name="seasonStartYear">Season to check</param>
        /// <returns>True if all games exist, otherwise False</returns>
        private async Task<bool> SeasonGamesExist(int seasonStartYear)
        {
            var gameCount = await _gameRepo.GetGameCountInSeason(seasonStartYear);
            var seasonGameCount = await _nhlDataGetter.GetGameCountInSeason(seasonStartYear);
            return gameCount == seasonGameCount;
        }

        /// <summary>
        /// Gets a seasons worth of games. Only returns games that have not already been found.
        /// </summary>
        /// <param name="seasonstartYear">year of games to get</param>
        /// <param name="gameCount">Number of games to get</param>
        /// <returns>List of games from the start year</returns>
        private async Task<List<DbGame>> GetSeasonGames(int seasonStartYear, int gameCount)
        {
            await _gameRepo.CacheSeasonOfGames(seasonStartYear);

            var seasonGames = new List<DbGame>();
            DbGame game;
            // game ids start at 1
            for (int count = 1; count <= gameCount; count++)
            {
                var gameId = _nhlDataGetter.GetGameIdFrom(seasonStartYear, count);
                if (_gameRepo.GameExistsInCache(gameId))
                    continue;

                game = await _nhlDataGetter.GetGame(gameId);

                if(game.IsValid())
                    seasonGames.Add(game);
            }

            return seasonGames;
        }
    }
}
