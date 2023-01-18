using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakeGameDataResponse;
using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakeLiveDataResponse;
using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakeTeamResponse;

namespace ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakePlayerDataResponse
{
    public class FakePlayerDataResponse
    {
        public FakeGamePlayer[] roster { get; set; } = new FakeGamePlayer[] { };
        public FakeGameData gameData { get; set; } = new FakeGameData();
        public FakeLiveData liveData { get; set; } = new FakeLiveData();
        public string message { get; set; } = "Real Object";
    }
}
