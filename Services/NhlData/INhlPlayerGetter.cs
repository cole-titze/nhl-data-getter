using Entities.DbModels;
using Entities.Models;

namespace Services.NhlData
{
    public interface INhlPlayerGetter
	{
        Task<List<int>> GetPlayerIdsForTeamBySeason(int seasonStartYear, string teamAbbr);
        Task<DbPlayer> GetPlayerValueBySeason(int playerId, int seasonStartYear);
        Task<Roster> GetGameRoster(DbGame game);
    }
}

