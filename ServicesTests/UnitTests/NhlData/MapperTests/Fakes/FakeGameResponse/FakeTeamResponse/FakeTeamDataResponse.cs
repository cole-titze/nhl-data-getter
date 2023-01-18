using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakeLiveDataResponse;

namespace ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakeTeamResponse
{
    public class FakeTeamDataResponse
    {
        public FakeTeam[] teams { get; set; } = new FakeTeam[] { new FakeTeam() };
    }
}
