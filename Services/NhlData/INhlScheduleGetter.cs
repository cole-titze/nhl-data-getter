namespace Services.NhlData
{
    public interface INhlScheduleGetter
	{
        Task<int> GetGameCountInSeason(int year);
        Dictionary<int, int> GetSeasonGameCounts();
        Task<List<int>> GetTeamsForSeason(int seasonStartYear);
    }
}

