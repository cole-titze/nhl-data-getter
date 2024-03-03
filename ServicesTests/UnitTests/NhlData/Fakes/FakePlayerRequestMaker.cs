//using Services.RequestMaker;
//using ServicesTests.UnitTests.NhlData.MapperTests;
//using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakePlayerBioResponse;
//using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakePlayerDataResponse;
//using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakePlayerStatResponse;
//using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakeTeamResponse;
//using static System.Net.WebRequestMethods;

//namespace ServicesTests.UnitTests.NhlData.Fakes
//{
//    public class FakePlayerRequestMaker : IRequestMaker
//    {
//        int rosterCount = 15;
//        public int teamCallCount = 0;
//        public static int invalidGameId = 2021020001;
//        public static string invalidTeamAbbr = "TOR";
//        public static int invalidPositionId = 19;
//        public static int invalidPeopleId = 10;
//        public static int validGameId = 2021020002;
//        public static string validTeamAbbr = "PIT";
//        public static int validPeopleId = 7;
//        string teamBadRequestUrl = "https://api-web.nhle.com/v1/roster/" + invalidTeamAbbr + "/current";
//        string teamGoodRequestUrl = "https://api-web.nhle.com/v1/roster/" + validTeamAbbr + "/current";
//        string peopleBadRequestUrl = "https://api.nhle.com/stats/rest/en/skater/summary?cayenneExp=seasonId=20232024%20and%20playerId=" + invalidPeopleId.ToString();
//        string peopleGoodRequestUrl = "https://api.nhle.com/stats/rest/en/skater/summary?cayenneExp=seasonId=20232024%20and%20playerId=" + validPeopleId.ToString();
//        string homeTeamUrl = "https://api-web.nhle.com/v1/roster/DAL/current";
//        string awayTeamUrl = "https://api-web.nhle.com/v1/roster/VGK/current";
//        string gameRosterUrl = "https://api-web.nhle.com/v1/gamecenter/2021020002/boxscore";

//        public Task<dynamic?> MakeRequest(string url, string query)
//        {
//            dynamic? response = null;
//            // Valid request to get players
//            if(url==peopleGoodRequestUrl && query != "")
//            {
//                double givenFaceOffPct = 60;
//                int pim = 6;
//                int plusMinus = -3;
//                int games = 10;
//                int blocked = 15;
//                int shots = 20;
//                int assists = 3;
//                int goals = 4;

//                response = new FakePlayerStatDataResponse();
//                response.stats = new List<FakeStats>() { new FakeStats() };
//                response.stats[0].splits[0].stat.faceOffPct = givenFaceOffPct;
//                response.stats[0].splits[0].stat.pim = pim;
//                response.stats[0].splits[0].stat.plusMinus = plusMinus;
//                response.stats[0].splits[0].stat.games = games;
//                response.stats[0].splits[0].stat.blocked = blocked;
//                response.stats[0].splits[0].stat.shots = shots;
//                response.stats[0].splits[0].stat.assists = assists;
//                response.stats[0].splits[0].stat.goals = goals;
//                return Task.FromResult<dynamic?>(response);
//            }
//            if (url == peopleGoodRequestUrl && query == "")
//            {
//                string expectedPrimaryPosition = "RW";
//                response = new FakePlayerBioData();
//                response.people[0].primaryPosition.abbreviation = expectedPrimaryPosition; return Task.FromResult<dynamic?>(response);
//            }
//            if (url == teamBadRequestUrl)
//            {
//                response = null;
//                return Task.FromResult<dynamic?>(response);
//            }
//            if (url == teamGoodRequestUrl)
//            {
//                response = MapPlayerResponseToPlayerIdsTests.GetFullMessage();
//                return Task.FromResult<dynamic?>(response);
//            }

//            if (url == homeTeamUrl || url == awayTeamUrl)
//            {
//                teamCallCount++;
//                response = new FakePlayerDataResponse();

//                var players = new List<FakeGamePlayer>();
//                for (int i = 0; i < rosterCount; i++)
//                {
//                    var player = new FakeGamePlayer();
//                    players.Add(player);
//                }
//                response.roster = players.ToArray();
//                return Task.FromResult<dynamic?>(response);
//            }
//            if (url == gameRosterUrl)
//            {
//                response = MapRosterResponseToGameRosterTests.GetFullMessage();
//                return Task.FromResult<dynamic?>(response);
//            }
            
//            return Task.FromResult<dynamic?>(response);
//        }
//    }
//}
