using DataAccess.GameRepository;
using Entities.DbModels;
using Entities.Types;
using Services.NhlData;

namespace BusinessLogic.GameGetter
{
	public class GameGetter
	{
        private IGameRepository _gameRepo;
        private INhlDataGetter _nhlDataGetter;
        public GameGetter(IGameRepository gameRepository, INhlDataGetter nhlDataGetter)
        {
            _gameRepo = gameRepository;
            _nhlDataGetter = nhlDataGetter;
        }
        /// <summary>
        /// Gets all nhl games within the season range. If the game is already in the database, it is skipped.
        /// </summary>
        public async Task GetGames(YearRange seasonYearRange)
        {
            for (int seasonStartYear = seasonYearRange.StartYear; seasonStartYear <= seasonYearRange.EndYear; seasonStartYear++)
            {
                if (await SeasonGamesExist(seasonStartYear))
                    continue;

                var seasonGameCount = await _nhlDataGetter.GetGameCountInSeason(seasonStartYear);
                var seasonGames = await GetSeasonGames(seasonStartYear, seasonGameCount);
                await _gameRepo.AddGames(seasonGames);
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
            for (int count = 0; count <= gameCount; count++)
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
