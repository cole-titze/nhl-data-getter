using System;
using Entities.DbModels;

namespace DataAccess.GameRepository
{
    public interface IGameRepository
    {
        Task AddGames(List<DbGame> seasonGames);
        Task CacheSeasonOfGames(int seasonStartYear);
        bool GameExistsInCache(int gameId);
        Task<int> GetGameCountInSeason(int year);
    }
}

