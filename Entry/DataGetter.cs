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
using DataAccess.TeamRepository;

namespace Entry
{
	public class DataGetter
	{
        private const int START_YEAR = 2010;
        private readonly ILogger<DataGetter> _logger;
        private readonly ILoggerFactory _loggerFactory;

        public DataGetter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DataGetter>();
            _loggerFactory = loggerFactory;
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
            var teamRepo = new TeamRepository(nhlDbContext);
            var requestMaker = new RequestMaker(new HttpClientWrapper());

            var seasonGameCountCache = await gameRepo.GetSeasonGameCounts();
            INhlGameGetter gameDataGetter = new NhlGameGetter(requestMaker, _loggerFactory);
            INhlScheduleGetter scheduleDataGetter = new NhlScheduleGetter(requestMaker, seasonGameCountCache, _loggerFactory);
            INhlPlayerGetter playerDataGetter = new NhlPlayerGetter(requestMaker, _loggerFactory);

            var nhlRequestMaker = new NhlDataGetter(gameDataGetter, playerDataGetter, scheduleDataGetter);
            var yearRange = new YearRange(START_YEAR, DateTime.Now);
            var playerYearRange = new YearRange(START_YEAR - 1, DateTime.Now); // We use player values from previous season for team ratings

            _logger.LogTrace("Starting Player Getter");
            var playerGetter = new PlayerGetter(playerRepo, teamRepo, nhlRequestMaker, _loggerFactory);
            await playerGetter.GetPlayers(playerYearRange);
            _logger.LogTrace("Completed Player Getter");

            _logger.LogTrace("Starting Game Getter");
            var gameGetter = new GameGetter(gameRepo, nhlRequestMaker, _loggerFactory);
            await gameGetter.GetGames(yearRange);

            watch.Stop();
            var elapsedTime = watch.Elapsed;
            var minutes = elapsedTime.TotalMinutes.ToString();
            _logger.LogTrace("Completed Game Getter in " + minutes + " minutes");
        }
	}
}

