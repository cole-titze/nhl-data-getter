using FluentAssertions;
using Services.NhlData.Mappers;
using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakeTeamResponse;
using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakeLiveDataResponse;
using Microsoft.CSharp.RuntimeBinder;

namespace ServicesTests.UnitTests.NhlData.MapperTests
{
    [TestClass]
    public class MapTeamResponseToTeamIdsTest
    {
        [TestMethod]
        public void CallToCut_WithNullResponse_ShouldThrowError()
        {
            dynamic message = new FakeTeamDataResponse();
            message.teams = null;


            Action testMap = () => MapTeamResponseToTeamIds.Map(message);

            Assert.ThrowsException<NullReferenceException>(testMap);
        }
        [TestMethod]
        public void CallToCut_WithInvalidResponse_ShouldThrowError()
        {
            dynamic message = "";

            Action testMap = () => MapTeamResponseToTeamIds.Map(message);

            Assert.ThrowsException<RuntimeBinderException>(testMap);
        }
        [TestMethod]
        public void CallToCut_WithZeroTeams_ShouldReturnZeroIds()
        {
            int expectedTeamCount = 0;
            dynamic message = new FakeTeamDataResponse();
            message.teams = new FakeTeam[0];

            List<int> teamIds = MapTeamResponseToTeamIds.Map(message);

            teamIds.Count.Should().Be(expectedTeamCount);
        }
        [TestMethod]
        public void CallToCut_WithOneTeam_ShouldReturnOneCorrectId()
        {
            int expectedTeamCount = 1;
            int[] expectedTeamIds = { 1 };

            dynamic message = new FakeTeamDataResponse();

            var teams = new List<FakeTeam>();
            for (int i = 0; i < expectedTeamCount; i++)
            {
                var team = new FakeTeam() { id = expectedTeamIds[i] };
                teams.Add(team);
            }
            message.teams = teams.ToArray();

            List<int> teamIds = MapTeamResponseToTeamIds.Map(message);

            for (int i = 0; i < expectedTeamCount; i++)
            {
                teamIds[i].Should().Be(expectedTeamIds[i]);
            }

            teamIds.Count.Should().Be(expectedTeamCount);
        }
        [TestMethod]
        public void CallToCut_WithFiveTeams_ShouldReturnFiveCorrectIds()
        {
            int expectedTeamCount = 5;
            int[] expectedTeamIds = { 1, 23897, 322, 33, 21 };

            dynamic message = new FakeTeamDataResponse();

            var teams = new List<FakeTeam>();
            for (int i = 0; i < expectedTeamCount; i++)
            {
                var team = new FakeTeam() { id = expectedTeamIds[i] };
                teams.Add(team);
            }
            message.teams = teams.ToArray();

            List<int> teamIds = MapTeamResponseToTeamIds.Map(message);

            for (int i = 0; i < expectedTeamCount; i++)
            {
                teamIds[i].Should().Be(expectedTeamIds[i]);
            }

            teamIds.Count.Should().Be(expectedTeamCount);
        }
    }
}
