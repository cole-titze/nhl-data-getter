using Entities.DbModels;
using Services.RequestMaker;
using Services.NhlData.Mappers;
using Microsoft.Extensions.Logging;
using System;

namespace Services.NhlData
{
    public partial class NhlApiDataGetter : INhlDataGetter
    {
        private readonly IRequestMaker _requestMaker;
        private readonly ILogger _logger;
        private const int DEFAULT_GAME_COUNT = 1400;
        private const string SEASON_TYPE = "02"; // 02 is the regular season id for nhl api
        private Dictionary<int, int> _seasonGameCountCache = new Dictionary<int, int>();
        public NhlApiDataGetter(IRequestMaker requestMaker, ILogger logger)
        {
            _requestMaker = requestMaker;
            _logger = logger;
        }
        /// <summary>
        /// Builds a game id to the nhl api standard "year""gameType"""gameId"
        /// (ex. 2022020001    year=2022 gameType=02 (02 is regular season) gameId=0001)
        /// </summary>
        /// <param name="seasonstartYear">Year to use in id</param>
        /// <param name="gameNumber">Game number to use in id</param>
        /// <returns>The internal game id</returns>
        public int GetGameIdFrom(int seasonstartYear, int gameNumber)
        {
            return (seasonstartYear * 1000000) + 20000 + gameNumber;
        }
        /// <summary>
        /// Gets the full season id used by the Nhl Api
        /// </summary>
        /// <param name="seasonStartYear"></param>
        /// <returns></returns>
        public int GetFullSeasonId(int seasonStartYear)
        {
            int nextYear = seasonStartYear + 1;
            return (seasonStartYear * 10000) + nextYear;
        }
    }

    public partial class NhlApiDataGetter : INhlGameGetter
    {
        /// <summary>
        /// Calls the Nhl api and parses the response into a game.
        /// </summary>
        /// <param name="gameId">The game to get</param>
        /// <returns>A game object corresponding to the id passed in</returns>
        /// Example Request: http://statsapi.web.nhl.com/api/v1/game/2019020001/feed/live
        public async Task<DbGame> GetGame(int gameId)
        {
            string url = "http://statsapi.web.nhl.com/api/v1/game/";
            string query = GetGameQuery(gameId);
            var gameResponse = await _requestMaker.MakeRequest(url, query);
            if (gameResponse == null)
            {
                _logger.LogWarning("Failed to get game with id: " + gameId.ToString());
                return new DbGame();
            }
            if (InvalidGame(gameResponse))
                return new DbGame();

            return MapGameResponseToGame.Map(gameResponse);
        }
        /// <summary>
        /// If game is not over, null was found, or both faceoffs were 0 the game is invalid
        /// </summary>
        /// <param name="message">response from nhl api</param>
        /// <returns></returns>
        private bool InvalidGame(dynamic message)
        {
            if (message.gameData.status.detailedState != "Final" && message.gameData.status.detailedState != "Scheduled")
                return true;

            return false;
        }
        /// <summary>
        /// Creates the game query
        /// </summary>
        /// <param name="seasonStartYear"></param>
        /// <param name="id"></param>
        /// <returns>Game query string</returns>
        private string GetGameQuery(int id)
        {
            string urlParameters = $"{id}/feed/live";

            return urlParameters;
        }
        /// <summary>
        /// Gets query for making schedule request to get all games.
        /// </summary>
        /// <param name="year">Season start year</param>
        /// <returns>A query for getting regular season games for a season</returns>
        private string GetTotalGamesQuery(int year)
        {
            var fullSeasonId = GetFullSeasonId(year);
            return $"?gameType=R&season={fullSeasonId}";
        }
    }

    public partial class NhlApiDataGetter
    {
        /// <summary>
        /// Calls to the NHL's api to get the schedule response.
        /// Parses the response to get the maximum id which is used as the count
        /// </summary>
        /// <param name="seasonStartYear">year of season to use</param>
        /// <returns>number of games in the season</returns>
        /// Example Request: https://statsapi.web.nhl.com/api/v1/schedule?gameType=R&season=20212022
        public async Task<int> GetGameCountInSeason(int seasonStartYear)
        {
            if (_seasonGameCountCache.ContainsKey(seasonStartYear))
                return _seasonGameCountCache[seasonStartYear];

            string url = "https://statsapi.web.nhl.com/api/v1/schedule";
            string query = GetTotalGamesQuery(seasonStartYear);
            var scheduleResponse = await _requestMaker.MakeRequest(url, query);
            if (scheduleResponse == null)
            {
                _logger.LogWarning("Schedule request failed, using default game count: " + DEFAULT_GAME_COUNT.ToString());
                return DEFAULT_GAME_COUNT;
            }
            _seasonGameCountCache[seasonStartYear] = MapScheduleToGameCount.Map(scheduleResponse);
            return _seasonGameCountCache[seasonStartYear];
        }
    }

    public partial class NhlApiDataGetter : INhlPlayerGetter
    {
        private Dictionary<int, List<DbGamePlayer>> _cachedTeamRoster = new Dictionary<int, List<DbGamePlayer>>();
        /// <summary>
        /// Gets a list of players mapped to games (DbGameRoster)
        /// </summary>
        /// <param name="game">Game to get players from</param>
        /// <returns>List of players from the game</returns>
        /// <exception cref="NotImplementedException"></exception>
        /// Example Request: http://statsapi.web.nhl.com/api/v1/game/2019020001/feed/live
        public async Task<List<DbGamePlayer>> GetGameRoster(DbGame game)
        {
            string url = "http://statsapi.web.nhl.com/api/v1/game/" + game.id.ToString() + "/feed/live";
            string query = "";

            var rosterResponse = await _requestMaker.MakeRequest(url, query);
            if (rosterResponse == null)
            {
                _logger.LogWarning($"Failed to get roster from request: Season: {game.seasonStartYear} Game: {game.id}");
                return new List<DbGamePlayer>();
            }
            List<DbGamePlayer> players = MapRosterResponseToGameRoster.MapPlayedGame(rosterResponse);

            if(players.Count() == 0)
            {
                if (_cachedTeamRoster.ContainsKey(game.homeTeamId))
                {
                    players.AddRange(_cachedTeamRoster[game.homeTeamId]);
                }
                else
                {
                    url = "https://statsapi.web.nhl.com/api/v1/teams/" + game.homeTeamId.ToString() + "/roster";
                    var homeResponse = await _requestMaker.MakeRequest(url, query);
                    if (homeResponse == null)
                    {
                        _logger.LogWarning($"Failed to get roster from request: Season: {game.seasonStartYear} Game: {game.id}");
                        return new List<DbGamePlayer>();
                    }
                    var teamRoster = MapRosterResponseToGameRoster.MapTeamRoster(homeResponse, game, game.homeTeamId);
                    players.AddRange(teamRoster);
                    _cachedTeamRoster.Add(game.homeTeamId, teamRoster);
                }
                if (_cachedTeamRoster.ContainsKey(game.awayTeamId))
                {
                    players.AddRange(_cachedTeamRoster[game.awayTeamId]);
                }
                else
                {
                    url = "https://statsapi.web.nhl.com/api/v1/teams/" + game.awayTeamId.ToString() + "/roster";
                    var awayResponse = await _requestMaker.MakeRequest(url, query);

                    if (awayResponse == null)
                    {
                        _logger.LogWarning($"Failed to get roster from request: Season: {game.seasonStartYear} Game: {game.id}");
                        return new List<DbGamePlayer>();
                    }
                    var teamRoster = MapRosterResponseToGameRoster.MapTeamRoster(awayResponse, game, game.awayTeamId);
                    players.AddRange(teamRoster);
                    _cachedTeamRoster.Add(game.awayTeamId, teamRoster);
                }
            }

            return players;
        }

        /// <summary>
        /// Gets a list of player ids for a team in a given season
        /// </summary>
        /// <param name="seasonStartYear">Season to get roster from</param>
        /// <param name="teamId">Team to get players from</param>
        /// <returns>List of player ids</returns>
        /// Ex. https://statsapi.web.nhl.com/api/v1/teams/1/roster?season=20112012
        public async Task<List<int>> GetPlayerIdsForTeamBySeason(int seasonStartYear, int teamId)
        {
            string url = "https://statsapi.web.nhl.com/api/v1/teams/" + teamId.ToString() + "/";
            string query = "roster?season=" + GetFullSeasonId(seasonStartYear).ToString();
            var playersResponse = await _requestMaker.MakeRequest(url, query);
            if (playersResponse == null)
            {
                _logger.LogWarning($"Failed to get player ids from team request: Season: {seasonStartYear} Team: {teamId}");
                return new List<int>();
            }

            return MapPlayerResponseToPlayerIds.Map(playersResponse);
        }
        /// <summary>
        /// Gets a players value
        /// </summary>
        /// <param name="playerId">Player id</param>
        /// <param name="seasonStartYear">Season to get value from</param>
        /// <returns>Player value</returns>
        /// Ex. https://statsapi.web.nhl.com/api/v1/people/8466141/stats?stats=statsSingleSeason&season=20152016
        /// Ex. https://statsapi.web.nhl.com/api/v1/people/8466141
        public async Task<DbPlayer> GetPlayerValueBySeason(int playerId, int seasonStartYear)
        {
            string url = "https://statsapi.web.nhl.com/api/v1/people/" + playerId.ToString() + "/";
            string query = "stats?stats=statsSingleSeason&season=" + GetFullSeasonId(seasonStartYear).ToString();
            var playerStatResponse = await _requestMaker.MakeRequest(url, query);
            if (playerStatResponse == null)
            {
                _logger.LogWarning($"Player stat request failed: player id: {playerId} year: {seasonStartYear}");
                return new DbPlayer();
            }

            DbPlayer player = MapPlayerStatResponseToPlayer.Map(playerStatResponse);
            player.seasonStartYear = seasonStartYear;

            query = "";
            var playerPositionResponse = await _requestMaker.MakeRequest(url, query);
            if (playerPositionResponse == null)
            {
                _logger.LogWarning($"Player position request failed: player id: {playerId} year: {seasonStartYear}");
                return new DbPlayer();
            }
            player.position = MapPlayerBioResponseToPositionStr.Map(playerPositionResponse);
            player.name = MapPlayerBioResponseToName.Map(playerPositionResponse);
            player.id = playerId;

            return player;
        }
        /// <summary>
        /// Gets a list of team ids from the season start year
        /// </summary>
        /// <param name="seasonStartYear">Year to get teams from</param>
        /// <returns>List of team ids</returns>
        /// Ex. https://statsapi.web.nhl.com/api/v1/teams?season=20112012
        public async Task<List<int>> GetTeamsForSeason(int seasonStartYear)
        {
            int seasonId = GetFullSeasonId(seasonStartYear);
            string url = "https://statsapi.web.nhl.com/api/v1/teams";
            string query = "?season="+seasonId.ToString();
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
