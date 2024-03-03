using Microsoft.EntityFrameworkCore;
using Entities.DbModels;

namespace DataAccess.TeamRepository
{
    public class TeamRepository : ITeamRepository
    {
        private readonly NhlDbContext _dbContext;
        public TeamRepository(NhlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets all teams from the database
        /// </summary>
        /// <returns>Query to get all teams</returns>
        private IQueryable<DbTeam> GetAllTeamsQuery()
        {
            return _dbContext.Team.AsQueryable();
        }

        /// <summary>
        /// Gets a single team stats
        /// </summary>
        /// <param name="teamId">The team to get stats for</param>
        /// <returns>The team stats</returns>
        public async Task<DbTeam> GetTeam(int teamId)
        {
            return await GetAllTeamsQuery().Where(x => x.id == teamId).FirstAsync();
        }
    }
}
