using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakeLiveDataResponse;

namespace ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakeGameDataResponse
{
    public class FakeGameData
    {
        public FakeTeams? teams { get; set; } = new FakeTeams();
        public FakeGame? game { get; set; } = new FakeGame();
        public FakeDateTime? datetime { get; set; } = new FakeDateTime();
        public FakeStatus? status { get; set; } = new FakeStatus();
    }
}
