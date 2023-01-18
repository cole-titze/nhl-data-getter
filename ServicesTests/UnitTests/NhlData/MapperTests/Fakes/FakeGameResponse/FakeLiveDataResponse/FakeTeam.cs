using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakeTeamResponse;

namespace ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakeLiveDataResponse
{
    public class FakeTeam
    {
        public int id { get; set; }
        public FakeTeamStats? teamStats { get; set; } = new FakeTeamStats();
        public List<int> skaters { get; set; } = new List<int>();
        public List<int> goalies { get; set; } = new List<int>();
        public FakeTeamInfo team { get; set; } = new FakeTeamInfo();
    }
}
