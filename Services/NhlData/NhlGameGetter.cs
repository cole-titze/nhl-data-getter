using Entities.DbModels;
using Microsoft.Extensions.Logging;
using Services.NhlData.Mappers;
using Services.RequestMaker;

namespace Services.NhlData
{
    public class NhlGameGetter : INhlGameGetter
	{
        private readonly IRequestMaker _requestMaker;
        private readonly ILogger<NhlGameGetter> _logger;

        public NhlGameGetter(IRequestMaker requestMaker, ILoggerFactory loggerFactory)
        {
            _requestMaker = requestMaker;
            _logger = loggerFactory.CreateLogger<NhlGameGetter>();
        }
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
    }
}

