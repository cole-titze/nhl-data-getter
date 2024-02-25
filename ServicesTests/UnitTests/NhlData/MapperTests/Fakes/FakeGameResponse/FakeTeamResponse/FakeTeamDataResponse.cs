using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakeLiveDataResponse;

namespace ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakeTeamResponse
{
    public class FakeTeamDataResponse
    {
        public List<FakeTeam> data { get; set; } = new List<FakeTeam>();

        // Legacy api
        public FakeTeam[] teams { get; set; } = new FakeTeam[] { new FakeTeam() };
    }
}
