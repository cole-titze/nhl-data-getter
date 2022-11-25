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
        /// Updates games to the database and adds them if they don't exist
        /// </summary>
        /// <param name="games">List of games to add or update</param>
        /// <returns>None</returns>
        public async Task AddUpdateGames(List<DbGame> games)
        {
            var addList = new List<DbGame>();
            var updateList = new List<DbGame>();
            foreach(var game in games)
            {
                var dbGame = _cachedSeasonsGames.FirstOrDefault(x => x.id == game.id);
                if (dbGame == null)
                    addList.Add(game);
                else
                {
                    dbGame.Clone(game);
                    updateList.Add(dbGame);
                }
            }
            await _dbContext.Game.AddRangeAsync(addList);
            _dbContext.Game.UpdateRange(updateList);
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
        /// Adds player rosters to the database if they don't exist. Removes players that are no longer on the roster for the game
        /// </summary>
        /// <param name="rosters">List of players mapped to games</param>
        /// <returns>None</returns>
        public async Task AddUpdateRosters(Dictionary<int, List<DbGamePlayer>> rosters)
        {
            List<DbGamePlayer> oldRosters = new List<DbGamePlayer>();
            foreach(var key in rosters.Keys)
            {
                oldRosters.AddRange(_dbContext.GamePlayer.Where(x => x.gameId == key));
            }

            var addList = new List<DbGamePlayer>();
            DbGamePlayer? dbPlayer;
            foreach (var roster in rosters)
            {
                foreach(var player in roster.Value)
                {
                    dbPlayer = oldRosters.FirstOrDefault(x => x.gameId == player.gameId && x.playerId == player.playerId);
                    if (dbPlayer == null)
                        addList.Add(player);
                    else
                        oldRosters.Remove(player);
                }
            }

            await _dbContext.GamePlayer.AddRangeAsync(addList);
            _dbContext.GamePlayer.RemoveRange(oldRosters);
        }
        /// <summary>
        /// Saves Database changes
        /// </summary>
        /// <returns>None</returns>
        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }

        public DbGame GetGame(int gameId)
        {
            var game = _cachedSeasonsGames.FirstOrDefault(x => x.id == gameId);
            if (game == null)
                return new DbGame();

            return game;
        }
    }
}

