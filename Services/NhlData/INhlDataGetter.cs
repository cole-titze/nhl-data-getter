using Entities.DbModels;

namespace Services.NhlData
{
    public interface INhlDataGetter : INhlGameGetter, INhlScheduleGetter, INhlPlayerGetter
    {
        int GetGameIdFrom(int seasonstartYear, int gameCount);
        int GetFullSeasonId(int seasonStartYear);
    }
}
