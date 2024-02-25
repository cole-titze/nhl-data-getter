using Microsoft.Extensions.Logging;
using Services.NhlData.Mappers;
using Services.RequestMaker;

namespace Services.NhlData
{
    public class NhlScheduleGetter : INhlScheduleGetter
	{
        private readonly IRequestMaker _requestMaker;
        private readonly ILogger<NhlGameGetter> _logger;
        private Dictionary<int, int> _seasonGameCountCache = new Dictionary<int, int>();
        private const int DEFAULT_GAME_COUNT = 1400;

        public NhlScheduleGetter(IRequestMaker requestMaker, Dictionary<int, int> seasonGameCountCache, ILoggerFactory loggerFactory)
        {
            _requestMaker = requestMaker;
            _logger = loggerFactory.CreateLogger<NhlGameGetter>();
            _seasonGameCountCache = seasonGameCountCache;
        }

        /// <summary>
        /// Calls to the NHL's api to get the schedule response.
        /// Parses the response to get the maximum id which is used as the count
        /// </summary>
        /// <param name="seasonStartYear">year of season to use</param>
        /// <returns>number of games in the season</returns>
        /// Example Request: https://api.nhle.com/stats/rest/en/season
        public async Task<int> GetGameCountInSeason(int seasonStartYear)
        {
            if (_seasonGameCountCache.ContainsKey(seasonStartYear))
                return _seasonGameCountCache[seasonStartYear];

            string url = "https://api.nhle.com/stats/rest/en/season";
            var scheduleResponse = await _requestMaker.MakeRequest(url, "");
            if (scheduleResponse == null)
            {
                _logger.LogWarning("Schedule request failed, using default game count: " + DEFAULT_GAME_COUNT.ToString());
                return DEFAULT_GAME_COUNT;
            }
            _seasonGameCountCache[seasonStartYear] = MapScheduleToGameCount.Map(scheduleResponse, NhlDataGetter.GetFullSeasonId(seasonStartYear));
            return _seasonGameCountCache[seasonStartYear];
        }
        /// <summary>
        /// Gets the seasons game counts
        /// </summary>
        /// <returns>Dictionary of year mapped to game count</returns>
        public Dictionary<int, int> GetSeasonGameCounts()
        {
            return _seasonGameCountCache;
        }
        /// <summary>
        /// Gets a list of team ids from the season start year
        /// </summary>
        /// <param name="seasonStartYear">Year to get teams from</param>
        /// <returns>List of team ids</returns>
        /// Ex. https://api.nhle.com/stats/rest/en/team/summary?cayenneExp=gameTypeId=2%20and%20seasonId=20202021
        public async Task<List<int>> GetTeamsForSeason(int seasonStartYear)
        {
            int seasonId = NhlDataGetter.GetFullSeasonId(seasonStartYear);
            string url = "https://api.nhle.com/stats/rest/en/team/summary";
            string query = "?cayenneExp=gameTypeId=2%20and%20seasonId=" + seasonId.ToString();
            var teamResponse = await _requestMaker.MakeRequest(url, query);
            if (teamResponse == null)
            {
                _logger.LogWarning("Failed to get teams for season: " + seasonStartYear.ToString());
                return new List<int>();
            }

            return MapTeamResponseToTeamIds.Map(teamResponse);
        }
    }
}

