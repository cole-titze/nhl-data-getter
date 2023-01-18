using Services.RequestMaker;
using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse;

namespace ServicesTests.UnitTests.NhlData.Fakes
{
    public class FakeGameRequestMaker : IRequestMaker
    {
        public int invalidGameId = 2021020001;
        public int validGameId = 2021020002;

        public Task<dynamic?> MakeRequest(string url, string query)
        {
            dynamic? response = null;

            if (query == "2021020001/feed/live")
            {
                response = null;
                return Task.FromResult<dynamic?>(response);
            }
            response = new FakeGameResponse();
            if (query == "2021020002/feed/live")
            {
                response.gameData.status.detailedState = "Ongoing";
                return Task.FromResult<dynamic?>(response);
            }
            response.gameData.status.detailedState = "Final";
            response.gameData.game.season = "2021020100";
            response.gameData.datetime.dateTime = "02/27/2021";
            return Task.FromResult<dynamic?>(response);
        }
    }
}
