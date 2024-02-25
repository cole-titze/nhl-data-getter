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
        /// Example Request: https://api-web.nhle.com/v1/gamecenter/2023020204/boxscore
        public async Task<DbGame> GetGame(int gameId)
        { 
            string url = "http://api-web.nhle.com/v1/gamecenter/";
            string query = GetGameQuery(gameId);
            var gameResponse = await _requestMaker.MakeRequest(url, query);
            if (gameResponse == null)
            {
                _logger.LogWarning("Failed to get game with id: " + gameId.ToString());
                return new DbGame();
            }
            if (IsGameInProgress(gameResponse))
                return new DbGame();

            return MapGameResponseToGame.Map(gameResponse);
        }
        /// <summary>
        /// Determines if a game is in progress or not
        /// </summary>
        /// <param name="message">response from nhl api</param>
        /// <returns>True if game is in progress, otherwise false</returns>
        private static bool IsGameInProgress(dynamic message)
        {
            if (message.gameState != "OFF" && message.gameState != "FUT")
                return true;

            return false;
        }
        /// <summary>
        /// Creates the game query
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Game query string</returns>
        private static string GetGameQuery(int id)
        {
            string urlParameters = $"{id}/boxscore";

            return urlParameters;
        }
    }
}

