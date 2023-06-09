using Services.RequestMaker;
using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakeLiveDataResponse;
using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakeScheduleResponse;
using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakeTeamResponse;

namespace ServicesTests.UnitTests.NhlData.Fakes
{
    internal class FakeScheduleRequestMaker : IRequestMaker
    {
        public int InvalidYear = 2021;
        public int ValidYear = 2022;
        public string TeamUrl = "https://statsapi.web.nhl.com/api/v1/teams";
        public string ScheduleUrl = "https://statsapi.web.nhl.com/api/v1/schedule";
        public string ValidYearQuery = "?season=20222023";
        public string InvalidScheduleQuery = "?gameType=R&season=20212022";
        public string ValidScheduleQuery = "?gameType=R&season=20222023";

        public Task<dynamic?> MakeRequest(string url, string query)
        {
            dynamic? response = null;
            if(url == TeamUrl && query == ValidYearQuery)
            {
                response = new FakeTeamDataResponse() { teams = new FakeTeam[] { new FakeTeam(), new FakeTeam(), new FakeTeam() } };
                return Task.FromResult<dynamic?>(response);
            }
            if(url == ScheduleUrl && query == ValidScheduleQuery)
            {
                response = new FakeScheduleData();
                response.totalGames = 200;
                return Task.FromResult<dynamic?>(response);
            }

            return Task.FromResult<dynamic?>(response);
        }
    }
}
