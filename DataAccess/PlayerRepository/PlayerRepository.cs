using Entities.DbModels;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.PlayerRepository
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly NhlDbContext _dbContext;
        public PlayerRepository(NhlDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// Add Players to database if they don't exist, otherwise update them
        /// </summary>
        /// <param name="playersWithValues">List of players to store</param>
        /// <returns>None</returns>
        public async Task AddUpdatePlayers(List<DbPlayer> playersWithValues)
        {
            var addList = new List<DbPlayer>();
            var updateList = new List<DbPlayer>();
            foreach (var player in playersWithValues)
            {
                player.value = BuildPlayerValue(player);

                var dbPlayer = _dbContext.PlayerValue.FirstOrDefault(p => p.id == player.id && p.seasonStartYear == player.seasonStartYear);
                if (dbPlayer == null)
                    addList.Add(player);
                else
                {
                    dbPlayer.value = player.value;
                    dbPlayer.position = player.position;
                    updateList.Add(dbPlayer);
                }
            }
            _dbContext.PlayerValue.AddRange(addList);
            _dbContext.PlayerValue.UpdateRange(updateList);

            await _dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// Checks for a player value. If the player has a value of 0 attempts to use the previous years value, otherwise keeps 0.
        /// </summary>
        /// <param name="player">The player to fill the value for</param>
        /// <returns>The player with a filled value</returns>
        private double BuildPlayerValue(DbPlayer player)
        {
            if (player.value == 0)
            {
                var playerLastYear = _dbContext.PlayerValue.FirstOrDefault(p => p.id == player.id && p.seasonStartYear == player.seasonStartYear - 1);
                if (playerLastYear != null)
                    player.value = playerLastYear.value;
            }

            return player.value;
        }

        /// <summary>
        /// Gets the number of players in the database for a given season.
        /// </summary>
        /// <param name="seasonStartYear">Year to get players for</param>
        /// <returns>Number of players found</returns>
        public async Task<int> GetPlayerCountBySeason(int seasonStartYear)
        {
            return await _dbContext.PlayerValue.Where(s => s.seasonStartYear == seasonStartYear).CountAsync();
        }
    }
}
