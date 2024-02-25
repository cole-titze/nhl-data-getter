using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakeTeamResponse;

namespace ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakeLiveDataResponse
{
    public class FakeTeam
    {
        public int id { get; set; }
        public string powerPlayConversion { get; set; } = "";
        public int score { get; set; }
        public int sog { get; set; }
        public int pim { get; set; }
        public double faceoffWinningPctg { get; set; }
        public int blocks { get; set; }
        public int hits { get; set; }
        public List<int> skaters { get; set; } = new List<int>();
        public List<int> goalies { get; set; } = new List<int>();

        // Legacy api
        public FakeTeamInfo team { get; set; } = new FakeTeamInfo();
        public FakeTeamStats? teamStats { get; set; } = new FakeTeamStats();
    }
}
