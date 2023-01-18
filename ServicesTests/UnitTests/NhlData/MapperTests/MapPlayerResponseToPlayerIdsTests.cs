using FluentAssertions;
using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakePlayerDataResponse;
using Services.NhlData.Mappers;
using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakePlayerBioResponse;
using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakeTeamResponse;

namespace ServicesTests.UnitTests.NhlData.MapperTests
{
    [TestClass]
    public class MapPlayerResponseToPlayerIdsTests
    {
        internal static FakePlayerDataResponse GetFullMessage()
        {
            var playerResponse = new FakePlayerDataResponse();
            playerResponse.roster = new FakeGamePlayer[] { new FakeGamePlayer { person = new FakePerson { id = 7 } }, new FakeGamePlayer { person = new FakePerson { id = 3 } } };

            return playerResponse;
        }
        [TestMethod]
        public void CallToCut_WithInvalidResponse_ShouldThrowError()
        {
            dynamic message = new FakePlayerBioData();
            message.roster = null;

            Action testMap = () => MapPlayerResponseToPlayerIds.Map(message);

            Assert.ThrowsException<NullReferenceException>(testMap);
        }
        [TestMethod]
        public void CallToCut_WithZeroPlayers_ShouldReturnCorrectPlayerId()
        {
            int expectedRosterCount = 0;

            var responsePerson = new FakePersonInfo();
            var person = new FakePerson() { person = responsePerson };
            dynamic message = new FakePlayerBioData();
            message.roster = new FakePerson[0];

            List<int> rosterIds = MapPlayerResponseToPlayerIds.Map(message);

            rosterIds.Count.Should().Be(expectedRosterCount);
        }
        [TestMethod]
        public void CallToCut_WithOnePlayer_ShouldReturnCorrectPlayerId()
        {
            int expectedRosterCount = 1;
            int expectedPlayerId = 7;

            var responsePerson = new FakePersonInfo() { id = expectedPlayerId };
            var person = new FakePerson() { person = responsePerson };
            dynamic message = new FakePlayerBioData();
            message.roster = new FakePerson[] { person };

            List<int> rosterIds = MapPlayerResponseToPlayerIds.Map(message);

            rosterIds.Count.Should().Be(expectedRosterCount);
            rosterIds[0].Should().Be(expectedPlayerId);
        }
        [TestMethod]
        public void CallToCut_WithFivePlayer_ShouldReturnCorrectPlayerIds()
        {
            int expectedRosterCount = 5;
            int[] expectedPlayerIds = { 7, 9, 21121, 2482398, 3 };

            List<FakePerson> people = new List<FakePerson>();
            dynamic message = new FakePlayerBioData();
            for (int i = 0; i < expectedRosterCount; i++)
            {
                var responsePerson = new FakePersonInfo() { id = expectedPlayerIds[i] };
                var person = new FakePerson() { person = responsePerson };
                people.Add(person);
            }
            message.roster = people.ToArray();

            List<int> rosterIds = MapPlayerResponseToPlayerIds.Map(message);

            rosterIds.Count.Should().Be(expectedRosterCount);

            for (int i = 0; i < expectedRosterCount; i++)
            {
                rosterIds[i].Should().Be(expectedPlayerIds[i]);
            }
        }
    }
}
