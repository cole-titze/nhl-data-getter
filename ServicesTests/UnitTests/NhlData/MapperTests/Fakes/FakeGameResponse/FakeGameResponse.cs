using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakeGameDataResponse;
using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakeLiveDataResponse;

namespace ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse
{
    public class FakeGameResponse
    {
        public FakeLiveData? liveData { get; set; } = new FakeLiveData();
        public FakeGameData? gameData { get; set; } = new FakeGameData();
        public int gamePk { get; set; }
    }
}
