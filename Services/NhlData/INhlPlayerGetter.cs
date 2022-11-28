using System;
using Entities.DbModels;
using Entities.Models;

namespace Services.NhlData
{
	public interface INhlPlayerGetter
	{
		Task<List<int>> GetTeamsForSeason(int seasonStartYear);
        Task<List<int>> GetPlayerIdsForTeamBySeason(int seasonStartYear, int teamId);
        Task<DbPlayer> GetPlayerValueBySeason(int playerId, int seasonStartYear);
        Task<Roster> GetGameRoster(DbGame game);
    }
}

