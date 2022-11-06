using DataAccess.GameRepository;
using DataAccess.PlayerRepository;
using DataAccess;
using BusinessLogic.GameGetter;
using Entities.Types;
using Services.RequestMaker;
using Services.NhlData;
using BusinessLogic.PlayerGetter;

namespace Entry
{
	public class DataGetter
	{
        private const int START_YEAR = 2010;

        /// <summary>
        /// Gets and stores all new games and player values.
        /// </summary>
        /// <param name="gamesConnectionString">db connection string</param>
        /// <returns></returns>
        public async Task Main(string gamesConnectionString)
        {
            // Run Data Collection

            var nhlDbContext = new NhlDbContext(gamesConnectionString);
            var playerRepo = new PlayerRepository(nhlDbContext);
            var gameRepo = new GameRepository(nhlDbContext);
            var requestMaker = new RequestMaker();
            var nhlRequestMaker = new NhlApiDataGetter(requestMaker);
            var yearRange = new YearRange(START_YEAR, DateTime.Now);

            Console.WriteLine("Starting Game Getter");
            var gameGetter = new GameGetter(gameRepo, nhlRequestMaker);
            await gameGetter.GetGames(yearRange);
            Console.WriteLine("Completed Game Getter");

            Console.WriteLine("Starting Player Getter");
            yearRange.StartYear = START_YEAR - 1; // We may want player values from previous season
            var playerGetter = new PlayerGetter(playerRepo, nhlRequestMaker);
            await playerGetter.GetPlayers(yearRange);
            Console.WriteLine("Completed Player Getter");
        }
	}
}

