using System;
using Entities.DbModels;

namespace DataAccess.GameRepository
{
    public interface IGameRepository
    {
        Task AddUpdateGames(List<DbGame> seasonGames);
        Task AddUpdateRosters(Dictionary<int, List<DbGamePlayer>> rosters);
        Task CacheSeasonOfGames(int seasonStartYear);
        Task Commit();
        bool GameExistsInCache(int gameId);
        DbGame GetGame(int gameId);
        Task<int> GetGameCountInSeason(int year);
    }
}

