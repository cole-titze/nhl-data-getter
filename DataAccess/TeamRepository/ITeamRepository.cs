using Entities.DbModels;

namespace DataAccess.TeamRepository
{
    public interface ITeamRepository
    {
        Task<DbTeam> GetTeam(int teamId);
    }
}

