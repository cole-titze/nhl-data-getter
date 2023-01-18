using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakePlayerBioResponse;

namespace ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakeTeamResponse
{
    public class FakeGamePlayer
    {
        public int gameId { get; set; }
        public int teamId { get; set; }
        public FakePerson person { get; set; } = new FakePerson();
        public int seasonStartYear { get; set; }
    }
}
