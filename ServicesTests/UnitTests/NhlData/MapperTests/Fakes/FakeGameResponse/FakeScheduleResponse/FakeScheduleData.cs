namespace ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakeScheduleResponse
{
    public class FakeScheduleData
    {
        public List<FakeData> data { get; set; } = new List<FakeData>();

        // Legacy api
        public int? totalGames { get; set; }
    }
}
