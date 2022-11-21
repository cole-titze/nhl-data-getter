using Entities.DbModels;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.GameRepository
{
    public class GameRepository : IGameRepository
    {
        private List<DbGame> _cachedSeasonsGames = new List<DbGame>();
        private readonly NhlDbContext _dbContext;
        public GameRepository(NhlDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// Gets total games for given season in database
        /// </summary>
        /// <param name="seasonStartYear">season start year</param>
        /// <returns>number of games in the season</returns>
        public async Task<int> GetGameCountInSeason(int seasonStartYear)
        {
            return await _dbContext.Game.Where(s => s.seasonStartYear == seasonStartYear).CountAsync();
        }
        /// <summary>
        /// Adds games to the database
        /// </summary>
        /// <param name="games">List of games to add</param>
        /// <returns>None</returns>
        public async Task AddGames(List<DbGame> games)
        {
            await _dbContext.Game.AddRangeAsync(games);
        }
        /// <summary>
        /// Gets a seasons worth of games and stores them in the cache variable
        /// </summary>
        /// <param name="seasonStartYear">Season start year</param>
        /// <returns>None</returns>
        public async Task CacheSeasonOfGames(int seasonStartYear)
        {
            _cachedSeasonsGames = await _dbContext.Game.Where(s => s.seasonStartYear == seasonStartYear).ToListAsync();
        }
        /// <summary>
        /// Gets if a game exists in cache
        /// </summary>
        /// <param name="gameId">Game to check</param>
        /// <returns>True if the game exists, otherwise false</returns>
        public bool GameExistsInCache(int gameId)
        {
            var game = _cachedSeasonsGames.FirstOrDefault(i => i.id == gameId);
            if (game == null)
                return false;
            return true;
        }
        /// <summary>
        /// Adds player rosters to the database
        /// </summary>
        /// <param name="rosters">List of players mapped to games</param>
        /// <returns>None</returns>
        public async Task AddRosters(List<DbGamePlayer> rosters)
        {
            var newRoster = rosters.GroupBy(x => new { x.gameId, x.playerId}).Select(x => x.First()).ToList(); //Remove duplicates if player played multiple positions in game

            await _dbContext.GamePlayer.AddRangeAsync(newRoster);
        }
        /// <summary>
        /// Saves Database changes
        /// </summary>
        /// <returns>None</returns>
        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}

