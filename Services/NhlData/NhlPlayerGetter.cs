using Entities.DbModels;
using Entities.Models;
using Entities.Types.Mappers;
using Microsoft.Extensions.Logging;
using Services.NhlData.Mappers;
using Services.RequestMaker;

namespace Services.NhlData
{
    public class NhlPlayerGetter : INhlPlayerGetter
	{
        private readonly IRequestMaker _requestMaker;
        private readonly ILogger<NhlPlayerGetter> _logger;
        private const int DEFAULT_GAME_COUNT = 1400;

        public NhlPlayerGetter(IRequestMaker requestMaker, ILoggerFactory loggerFactory)
        {
            _requestMaker = requestMaker;
            _logger = loggerFactory.CreateLogger<NhlPlayerGetter>();
        }
        private Dictionary<int, List<DbGamePlayer>> _cachedTeamRoster = new Dictionary<int, List<DbGamePlayer>>();
        /// <summary>
        /// Gets a list of players mapped to games (DbGameRoster)
        /// </summary>
        /// <param name="game">Game to get players from</param>
        /// <returns>List of players from the game</returns>
        /// <exception cref="NotImplementedException"></exception>
        /// Example Request: http://statsapi.web.nhl.com/api/v1/game/2019020001/feed/live
        public async Task<Roster> GetGameRoster(DbGame game)
        {
            Roster players = await GetPastGameRoster(game);

            if (players.homeTeam.Count() == 0 || players.awayTeam.Count() == 0)
            {
                players = await GetCurrentTeamRosters(game);
            }

            return players;
        }
        /// <summary>
        /// Get the current team roster
        /// </summary>
        /// <param name="game">Game to get rosters of</param>
        /// <returns>List of players that will play in the game</returns>
        private async Task<Roster> GetCurrentTeamRosters(DbGame game)
        {
            Roster players = new Roster();

            players.homeTeam = await GetTeamRoster(game, game.homeTeamId);
            players.awayTeam = await GetTeamRoster(game, game.awayTeamId);

            return players;
        }

        /// <summary>
        /// Gets roster for given team and game.
        /// </summary>
        /// <param name="game">Game to get players for</param>
        /// <param name="teamId">Team to get players for</param>
        /// <returns>List of players that played for the team</returns>
        private async Task<List<DbGamePlayer>> GetTeamRoster(DbGame game, int teamId)
        {
            var players = new List<DbGamePlayer>();
            string url;
            string query;

            if (_cachedTeamRoster.ContainsKey(teamId))
            {
                players.AddRange(_cachedTeamRoster[teamId]);
            }
            else
            {
                url = "https://statsapi.web.nhl.com/api/v1/teams/" + teamId.ToString() + "/roster";
                query = "";
                var teamResponse = await _requestMaker.MakeRequest(url, query);
                if (teamResponse == null)
                {
                    _logger.LogWarning($"Failed to get roster from request: Season: {game.seasonStartYear} Game: {game.id}");
                    return new List<DbGamePlayer>();
                }
                List<DbGamePlayer> teamRoster = MapRosterResponseToGameRoster.MapTeamRoster(teamResponse, game, teamId);
                players.AddRange(teamRoster);
                _cachedTeamRoster.Add(teamId, teamRoster);
            }
            return players;
        }

        /// <summary>
        /// Gets roster for games that have already been played.
        /// </summary>
        /// <param name="game">The game to get the roster of</param>
        /// <returns>List of players from a game</returns>
        private async Task<Roster> GetPastGameRoster(DbGame game)
        {
            string url = "http://statsapi.web.nhl.com/api/v1/game/" + game.id.ToString() + "/feed/live";
            string query = "";

            var rosterResponse = await _requestMaker.MakeRequest(url, query);
            if (rosterResponse == null)
            {
                _logger.LogWarning($"Failed to get roster from request: Season: {game.seasonStartYear} Game: {game.id}");
                return new Roster();
            }
            return MapRosterResponseToGameRoster.MapPlayedGame(rosterResponse);
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
            string query = "roster?season=" + NhlDataGetter.GetFullSeasonId(seasonStartYear).ToString();
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
            string query = "stats?stats=statsSingleSeason&season=" + NhlDataGetter.GetFullSeasonId(seasonStartYear).ToString();
            var playerStatResponse = await _requestMaker.MakeRequest(url, query);
            if (playerStatResponse == null)
            {
                _logger.LogWarning($"Player stat request failed: player id: {playerId} year: {seasonStartYear}");
                return new DbPlayer();
            }

            query = "";
            var playerPositionResponse = await _requestMaker.MakeRequest(url, query);
            if (playerPositionResponse == null)
            {
                _logger.LogWarning($"Player position request failed: player id: {playerId} year: {seasonStartYear}");
                return new DbPlayer();
            }
            IPlayerStats playerStats = MapPlayerStatResponseToPlayer.BuildPlayerStats(playerStatResponse);
            playerStats.position = MapPositionStrToPosition.Map(MapPlayerBioResponseToPositionStr.Map(playerPositionResponse));
            DbPlayer player = MapPlayerStatResponseToPlayer.MapPlayerStatsToPlayer(playerStats);
            player.seasonStartYear = seasonStartYear;
            player.name = MapPlayerBioResponseToName.Map(playerPositionResponse);
            player.id = playerId;

            return player;
        }
    }
}

