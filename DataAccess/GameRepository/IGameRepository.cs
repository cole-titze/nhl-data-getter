using System;
using Entities.DbModels;

namespace DataAccess.GameRepository
{
    public interface IGameRepository
    {
        Task AddGames(List<DbGame> seasonGames);
        Task AddRosters(List<DbGamePlayer> seasonGames);
        Task CacheSeasonOfGames(int seasonStartYear);
        Task Commit();
        bool GameExistsInCache(int gameId);
        Task<int> GetGameCountInSeason(int year);
    }
}

