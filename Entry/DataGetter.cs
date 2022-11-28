using DataAccess.GameRepository;
using DataAccess.PlayerRepository;
using DataAccess;
using BusinessLogic.GameGetter;
using Entities.Types;
using Services.RequestMaker;
using Services.NhlData;
using BusinessLogic.PlayerGetter;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Entry
{
	public class DataGetter
	{
        private const int START_YEAR = 2010;
        private readonly ILogger _logger;

        public DataGetter(ILogger logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Gets and stores all new games and player values.
        /// </summary>
        /// <param name="gamesConnectionString">db connection string</param>
        /// <returns>None</returns>
        public async Task Main(string gamesConnectionString)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));

            var nhlDbContext = new NhlDbContext(gamesConnectionString);
            var playerRepo = new PlayerRepository(nhlDbContext);
            var gameRepo = new GameRepository(nhlDbContext);
            var requestMaker = new RequestMaker();
            var nhlRequestMaker = new NhlApiDataGetter(requestMaker, _logger);
            var yearRange = new YearRange(START_YEAR, DateTime.Now);
            var playerYearRange = new YearRange(START_YEAR - 1, DateTime.Now); // We use player values from previous season for team ratings

            _logger.LogTrace("Starting Player Getter");
            var playerGetter = new PlayerGetter(playerRepo, nhlRequestMaker, _logger);
            await playerGetter.GetPlayers(playerYearRange);
            _logger.LogTrace("Completed Player Getter");

            _logger.LogTrace("Starting Game Getter");
            var gameGetter = new GameGetter(gameRepo, nhlRequestMaker, _logger);
            await gameGetter.GetGames(yearRange);

            watch.Stop();
            var elapsedTime = watch.Elapsed;
            var minutes = elapsedTime.TotalMinutes.ToString();
            _logger.LogTrace("Completed Game Getter in " + minutes + " minutes");
        }
	}
}

