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
        public string TeamUrl = "https://api.nhle.com/stats/rest/en/team/summary";
        public string ScheduleUrl = "https://api.nhle.com/stats/rest/en/season";
        public string ValidYearQuery = "?cayenneExp=gameTypeId=2%20and%20seasonId=20222023";
        public string InvalidScheduleQuery = "?cayenneExp=gameTypeId=2%20and%20seasonId=20212022";
        public string ValidScheduleQuery = "?cayenneExp=gameTypeId=2%20and%20seasonId=20222023";

        public Task<dynamic?> MakeRequest(string url, string query)
        {
            dynamic? response = null;
            if(url == TeamUrl && query == ValidYearQuery)
            {
                response = new FakeTeamDataResponse() { data = new List<FakeTeam> { new FakeTeam(), new FakeTeam(), new FakeTeam() } };
                return Task.FromResult<dynamic?>(response);
            }
            if(url == ScheduleUrl)
            {
                response = new FakeScheduleData();
                response.data = new List<FakeData>() { new FakeData(), new FakeData() };
                response.data[0].totalRegularSeasonGames = 200;
                response.data[0].id = 20222023;

                response.data[1].id = 20212022;
                response.data[1].totalRegularSeasonGames = 1400;

                return Task.FromResult<dynamic?>(response);
            }

            return Task.FromResult<dynamic?>(response);
        }
    }
}
