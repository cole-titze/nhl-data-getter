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

            if (query == "2021020001/boxscore")
            {
                response = null;
                return Task.FromResult<dynamic?>(response);
            }
            response = new FakeGameResponse();
            if (query == "2021020002/boxscore")
            {
                response.gameState = "LIVE";
                return Task.FromResult<dynamic?>(response);
            }
            response.gameState = "OFF";
            response.season = "2021020100";
            response.startTimeUTC = "02/27/2021";
            return Task.FromResult<dynamic?>(response);
        }
    }
}
