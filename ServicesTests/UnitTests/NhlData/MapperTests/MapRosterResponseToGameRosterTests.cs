using Entities.DbModels;
using Entities.Models;
using FluentAssertions;
using Microsoft.CSharp.RuntimeBinder;
using Services.NhlData.Mappers;
using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakePlayerDataResponse;
using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakeTeamResponse;

namespace ServicesTests.UnitTests.NhlData.MapperTests
{
    [TestClass]
    public class MapRosterResponseToGameRosterTests
    {
        internal static dynamic? GetFullMessage()
        {
            int[] expectedHomePlayerIds = { 3, 4, 32, 34524, 5323, 243424, 4235324, 23432, 34534, 23235, 2344232, 3245322, 2342, 23423, 1221, 9809, 90880, 98009, 908908, 9809, 89, 979090, 768, 34 };
            int[] expectedHomeGoalieIds = { 12, 32 };
            int[] expectedAwayPlayerIds = { 4, 3489934, 4334, 32, 328872377 };
            int[] expectedAwayGoalieIds = { 123, 129 };
            int homeTeamId = 5;
            int awayTeamId = 21;
            int expectedGameId = 3423532;
            dynamic message = MessageFactory(homeTeamId, awayTeamId, expectedGameId);

            var homePlayers = PlayerFactory(expectedHomePlayerIds);
            message.liveData.boxscore.teams.home.skaters = homePlayers;
            var homeGoalies = PlayerFactory(expectedHomeGoalieIds);
            message.liveData.boxscore.teams.home.goalies = homeGoalies;
            var awayPlayers = PlayerFactory(expectedAwayPlayerIds);
            message.liveData.boxscore.teams.away.skaters = awayPlayers;
            var awayGoalies = PlayerFactory(expectedAwayGoalieIds);
            message.liveData.boxscore.teams.away.goalies = awayGoalies;

            return message;
        }
        internal void CheckGameRoster(List<DbGamePlayer> team, int expectedPlayerCount, int teamId, int expectedSeasonStartYear, int expectedGameId, int[] expectedPlayerIds)
        {
            for (int i = 0; i < expectedPlayerCount; i++)
            {
                var player = team[i];
                player.gameId.Should().Be(expectedGameId);
                player.teamId.Should().Be(teamId);
                player.playerId.Should().Be(expectedPlayerIds[i]);
                player.seasonStartYear.Should().Be(expectedSeasonStartYear);
            }
        }
        internal static List<int> PlayerFactory(int[] expectedPlayerIds)
        {
            var players = new List<int>();
            for (int i = 0; i < expectedPlayerIds.Count(); i++)
            {
                players.Add(expectedPlayerIds[i]);
            }

            return players;
        }
        internal static dynamic MessageFactory(int homeTeamId, int awayTeamId, int gameId)
        {
            dynamic message = new FakePlayerDataResponse();
            message.gameData.game.season = "20212022";

            message.liveData.boxscore.teams.home.team.id = homeTeamId;
            message.liveData.boxscore.teams.away.team.id = awayTeamId;
            message.gameData.game.pk = gameId;

            return message;
        }
        [TestMethod]
        public void CallToMapTeamRoster_WithEmptyMessage_ShouldThrowError()
        {
            dynamic message = "";
            DbGame game = new DbGame() { homeTeamId = 12 };
            int teamId = game.homeTeamId;
            Action testMap = () => MapRosterResponseToGameRoster.MapTeamRoster(message, game, teamId);

            Assert.ThrowsException<RuntimeBinderException>(testMap);
        }
        [TestMethod]
        public void CallToMapTeamRoster_WithInvalidMessage_ShouldThrowError()
        {
            dynamic message = new FakePlayerDataResponse();
            message.roster = null;
            DbGame game = new DbGame() { homeTeamId = 12 };
            int teamId = game.homeTeamId;
            Action testMap = () => MapRosterResponseToGameRoster.MapTeamRoster(message, game, teamId);

            Assert.ThrowsException<NullReferenceException>(testMap);
        }
        [TestMethod]
        public void CallToMapTeamRoster_WithZeroPlayers_ShouldReturnEmptyRoster()
        {
            int expectedPlayerCount = 0;
            int[] expectedPlayerIds = { };
            int teamId = 8;
            DbGame game = new DbGame() { awayTeamId = teamId };

            dynamic message = new FakePlayerDataResponse();

            var players = new List<FakeGamePlayer>();
            message.roster = players.ToArray();

            List<DbGamePlayer> gamePlayers = MapRosterResponseToGameRoster.MapTeamRoster(message, game, teamId);

            gamePlayers.Count.Should().Be(expectedPlayerCount);
        }
        [TestMethod]
        public void CallToMapTeamRoster_WithOnePlayer_ShouldReturnCorrectRoster()
        {
            int expectedPlayerCount = 1;
            int[] expectedPlayerIds = { 4 };
            int expectedGameId = 234;
            int expectedSeasonStartYear = 2021;
            int teamId = 8;
            DbGame game = new DbGame() { awayTeamId = teamId, id = expectedGameId, seasonStartYear = expectedSeasonStartYear };

            dynamic message = new FakePlayerDataResponse();

            var players = new List<FakeGamePlayer>();
            for (int i = 0; i < expectedPlayerCount; i++)
            {
                var player = new FakeGamePlayer();
                player.gameId = expectedGameId;
                player.teamId = teamId;
                player.person.id = expectedPlayerIds[i];
                player.seasonStartYear = expectedSeasonStartYear;
                players.Add(player);
            }
            message.roster = players.ToArray();

            List<DbGamePlayer> gamePlayers = MapRosterResponseToGameRoster.MapTeamRoster(message, game, teamId);

            for (int i = 0; i < expectedPlayerCount; i++)
            {
                gamePlayers[i].playerId.Should().Be(expectedPlayerIds[i]);
                gamePlayers[i].gameId.Should().Be(expectedGameId);
                gamePlayers[i].seasonStartYear.Should().Be(expectedSeasonStartYear);
                gamePlayers[i].teamId.Should().Be(teamId);
            }

            gamePlayers.Count.Should().Be(expectedPlayerCount);
        }
        [TestMethod]
        public void CallToMapTeamRoster_WithFivePlayers_ShouldReturnCorrectRoster()
        {
            int expectedPlayerCount = 5;
            int[] expectedPlayerIds = { 4, 23432, 23, 88, 31223423 };
            int expectedGameId = 234;
            int expectedSeasonStartYear = 2021;
            int teamId = 8;
            DbGame game = new DbGame() { awayTeamId = teamId, id = expectedGameId, seasonStartYear = expectedSeasonStartYear };

            dynamic message = new FakePlayerDataResponse();

            var players = new List<FakeGamePlayer>();
            for (int i = 0; i < expectedPlayerCount; i++)
            {
                var player = new FakeGamePlayer();
                player.gameId = expectedGameId;
                player.teamId = teamId;
                player.person.id = expectedPlayerIds[i];
                player.seasonStartYear = expectedSeasonStartYear;
                players.Add(player);
            }
            message.roster = players.ToArray();

            List<DbGamePlayer> gamePlayers = MapRosterResponseToGameRoster.MapTeamRoster(message, game, teamId);

            for (int i = 0; i < expectedPlayerCount; i++)
            {
                gamePlayers[i].playerId.Should().Be(expectedPlayerIds[i]);
                gamePlayers[i].gameId.Should().Be(expectedGameId);
                gamePlayers[i].seasonStartYear.Should().Be(expectedSeasonStartYear);
                gamePlayers[i].teamId.Should().Be(teamId);
            }

            gamePlayers.Count.Should().Be(expectedPlayerCount);
        }
        [TestMethod]
        public void CallToMapPlayedGame_WithEmptySeason_ShouldThrowError()
        {
            dynamic message = new FakePlayerDataResponse();
            message.gameData.game.season = "";
            Action testMap = () => MapRosterResponseToGameRoster.MapPlayedGame(message);

            Assert.ThrowsException<ArgumentOutOfRangeException>(testMap);
        }
        [TestMethod]
        public void CallToMapPlayedGame_WithInvalidSeason_ShouldThrowError()
        {
            dynamic message = new FakePlayerDataResponse();
            message.gameData.game.season = "asdfas";
            Action testMap = () => MapRosterResponseToGameRoster.MapPlayedGame(message);

            Assert.ThrowsException<FormatException>(testMap);
        }
        [TestMethod]
        public void CallToMapPlayedGame_WithValidSeason_ShouldReturnEmptyRoster()
        {
            dynamic message = new FakePlayerDataResponse();
            message.gameData.game.season = "20212022";

            Roster gameRoster = MapRosterResponseToGameRoster.MapPlayedGame(message);

            gameRoster.awayTeam.Should().BeEmpty();
            gameRoster.homeTeam.Should().BeEmpty();
        }
        [TestMethod]
        public void CallToMapPlayedGame_WithValidResponse_ShouldReturnEmptyRoster()
        {
            int homeTeamId = 7;
            int awayTeamId = 31;
            int gameId = 3423532;
            dynamic message = MessageFactory(homeTeamId, awayTeamId, gameId);

            Roster gameRoster = MapRosterResponseToGameRoster.MapPlayedGame(message);

            gameRoster.awayTeam.Should().BeEmpty();
            gameRoster.homeTeam.Should().BeEmpty();
        }
        [TestMethod]
        public void CallToMapPlayedGame_WithValidResponseAndOnePlayer_ShouldReturnCorrectRoster()
        {
            int expectedHomePlayerCount = 1;
            int[] expectedHomePlayerIds = { 7 };
            int expectedAwayPlayerCount = 0;
            int[] expectedAwayPlayerIds = { };
            int expectedSeasonStartYear = 2021;
            int homeTeamId = 7;
            int awayTeamId = 31;
            int expectedGameId = 3423532;
            dynamic message = MessageFactory(homeTeamId, awayTeamId, expectedGameId);

            var homePlayers = PlayerFactory(expectedHomePlayerIds);
            message.liveData.boxscore.teams.home.skaters = homePlayers;
            var awayPlayers = PlayerFactory(expectedAwayPlayerIds);
            message.liveData.boxscore.teams.away.skaters = awayPlayers;

            Roster gameRoster = MapRosterResponseToGameRoster.MapPlayedGame(message);

            gameRoster.awayTeam.Should().BeEmpty();
            gameRoster.homeTeam.Should().HaveCount(expectedHomePlayerCount);
            CheckGameRoster(gameRoster.homeTeam, expectedHomePlayerCount, homeTeamId, expectedSeasonStartYear, expectedGameId, expectedHomePlayerIds);
            CheckGameRoster(gameRoster.awayTeam, expectedAwayPlayerCount, awayTeamId, expectedSeasonStartYear, expectedGameId, expectedAwayPlayerIds);
        }
        [TestMethod]
        public void CallToMapPlayedGame_WithValidResponseAndFivePlayers_ShouldReturnCorrectRosters()
        {
            int expectedHomePlayerCount = 0;
            int[] expectedHomePlayerIds = { };
            int expectedAwayPlayerCount = 5;
            int[] expectedAwayPlayerIds = { 4, 3489934, 4334, 32, 328872377 };
            int expectedSeasonStartYear = 2021;
            int homeTeamId = 5;
            int awayTeamId = 21;
            int expectedGameId = 3423532;
            dynamic message = MessageFactory(homeTeamId, awayTeamId, expectedGameId);

            var homePlayers = PlayerFactory(expectedHomePlayerIds);
            message.liveData.boxscore.teams.home.skaters = homePlayers;
            var awayPlayers = PlayerFactory(expectedAwayPlayerIds);
            message.liveData.boxscore.teams.away.skaters = awayPlayers;

            Roster gameRoster = MapRosterResponseToGameRoster.MapPlayedGame(message);

            gameRoster.awayTeam.Should().HaveCount(expectedAwayPlayerCount);
            gameRoster.homeTeam.Should().BeEmpty();
            CheckGameRoster(gameRoster.homeTeam, expectedHomePlayerCount, homeTeamId, expectedSeasonStartYear, expectedGameId, expectedHomePlayerIds);
            CheckGameRoster(gameRoster.awayTeam, expectedAwayPlayerCount, awayTeamId, expectedSeasonStartYear, expectedGameId, expectedAwayPlayerIds);
        }
        [TestMethod]
        public void CallToMapPlayedGame_WithValidResponseAndManyPlayers_ShouldReturnCorrectRosters()
        {
            int expectedHomePlayerCount = 24;
            int[] expectedHomePlayerIds = { 3, 4, 32, 34524, 5323, 243424, 4235324, 23432, 34534, 23235, 2344232, 3245322, 2342, 23423, 1221, 9809, 90880, 98009, 908908, 9809, 89, 979090, 768, 34 };
            int expectedAwayPlayerCount = 5;
            int[] expectedAwayPlayerIds = { 4, 3489934, 4334, 32, 328872377 };
            int expectedSeasonStartYear = 2021;
            int homeTeamId = 5;
            int awayTeamId = 21;
            int expectedGameId = 3423532;
            dynamic message = MessageFactory(homeTeamId, awayTeamId, expectedGameId);

            var homePlayers = PlayerFactory(expectedHomePlayerIds);
            message.liveData.boxscore.teams.home.skaters = homePlayers;
            var awayPlayers = PlayerFactory(expectedAwayPlayerIds);
            message.liveData.boxscore.teams.away.skaters = awayPlayers;

            Roster gameRoster = MapRosterResponseToGameRoster.MapPlayedGame(message);

            gameRoster.awayTeam.Should().HaveCount(expectedAwayPlayerCount);
            gameRoster.homeTeam.Should().HaveCount(expectedHomePlayerCount);
            CheckGameRoster(gameRoster.homeTeam, expectedHomePlayerCount, homeTeamId, expectedSeasonStartYear, expectedGameId, expectedHomePlayerIds);
            CheckGameRoster(gameRoster.awayTeam, expectedAwayPlayerCount, awayTeamId, expectedSeasonStartYear, expectedGameId, expectedAwayPlayerIds);
        }
        [TestMethod]
        public void CallToMapPlayedGame_WithValidResponseAndManyPlayersAndManyGoalies_ShouldReturnCorrectRosters()
        {
            dynamic? message = GetFullMessage();
            int expectedHomePlayerCount = 24;
            int[] expectedHomePlayerIds = { 3, 4, 32, 34524, 5323, 243424, 4235324, 23432, 34534, 23235, 2344232, 3245322, 2342, 23423, 1221, 9809, 90880, 98009, 908908, 9809, 89, 979090, 768, 34 };
            int expectedHomeGoalieCount = 2;
            int expectedAwayPlayerCount = 5;
            int[] expectedAwayPlayerIds = { 4, 3489934, 4334, 32, 328872377 };
            int expectedAwayGoalieCount = 2;
            int expectedSeasonStartYear = 2021;
            int homeTeamId = 5;
            int awayTeamId = 21;
            int expectedGameId = 3423532;

            Roster gameRoster = MapRosterResponseToGameRoster.MapPlayedGame(message);

            gameRoster.awayTeam.Should().HaveCount(expectedAwayPlayerCount + expectedAwayGoalieCount);
            gameRoster.homeTeam.Should().HaveCount(expectedHomePlayerCount + expectedHomeGoalieCount);
            CheckGameRoster(gameRoster.homeTeam, expectedHomePlayerCount, homeTeamId, expectedSeasonStartYear, expectedGameId, expectedHomePlayerIds);
            CheckGameRoster(gameRoster.awayTeam, expectedAwayPlayerCount, awayTeamId, expectedSeasonStartYear, expectedGameId, expectedAwayPlayerIds);
        }
    }
}
