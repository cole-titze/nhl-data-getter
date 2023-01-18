using Entities.DbModels;
using Entities.Models;

namespace DataAccess.GameRepository
{
    public interface IGameRepository
    {
        Task AddSeasonGameCounts(Dictionary<int, int> seasonGameCountCache);
        Task AddUpdateGames(List<DbGame> seasonGames);
        Task AddUpdateRosters(Dictionary<int, Roster> rosters);
        Task CacheSeasonOfGames(int seasonStartYear);
        Task Commit();
        bool GameExistsInCache(int gameId);
        DbGame GetGame(int gameId);
        Task<int> GetGameCountInSeason(int year);
        Task<Dictionary<int, int>> GetSeasonGameCounts();
    }
}

