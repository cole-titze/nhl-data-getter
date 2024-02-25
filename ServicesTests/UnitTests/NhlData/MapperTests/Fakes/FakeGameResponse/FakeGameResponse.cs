using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakeGameDataResponse;
using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakeLiveDataResponse;

namespace ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse
{
    public class FakeGameResponse
    {
        public FakeLiveData? liveData { get; set; } = new FakeLiveData();
        public FakeGameData? gameData { get; set; } = new FakeGameData();
        public FakeTeam? homeTeam { get; set; } = new FakeTeam();
        public FakeTeam? awayTeam { get; set; } = new FakeTeam();
        public int id { get; set; }
        public string gameState { get; set; } = "";
        public string season { get; set; } = "";
        public string startTimeUTC { get; set; } = "";
        public FakeLineScore linescore { get; set; } = new FakeLineScore();
    }
}
