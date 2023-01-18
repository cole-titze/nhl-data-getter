using Entities.DbModels;

namespace Services.NhlData
{
    public interface INhlGameGetter
	{
        Task<DbGame> GetGame(int gameId);
    }
}

