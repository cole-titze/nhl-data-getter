using System;
using Entities.DbModels;

namespace DataAccess.PlayerRepository
{
    public interface IPlayerRepository
    {
        Task AddUpdatePlayers(List<DbPlayer> players);
        Task<int> GetPlayerCountBySeason(int seasonStartYear);
    }
}

