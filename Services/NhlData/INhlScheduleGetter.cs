namespace Services.NhlData
{
    public interface INhlScheduleGetter
	{
        Task<int> GetGameCountInSeason(int year);
    }
}

