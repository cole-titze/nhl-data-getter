using Services.NhlData;
using ServicesTests.UnitTests.NhlData.Fakes;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using FluentAssertions;

namespace ServicesTests.UnitTests.NhlData
{
    [TestClass]
    public class NhlScheduleGetterTests
    {
        [TestMethod]
        public void CallToGetSeasonGameCounts_WithValidCache_ShouldGetCache()
        {
            var fakeRequestMaker = new FakeScheduleRequestMaker();
            Dictionary<int, int> cache = new Dictionary<int, int>();
            cache.Add(1, 1);

            var cut = new NhlScheduleGetter(fakeRequestMaker, cache, A.Fake<ILoggerFactory>());
            var newCache = cut.GetSeasonGameCounts();

            newCache[1].Should().Be(1);
        }
        [TestMethod]
        public async Task CallToGetTeamsForSeason_WithInvalidResponse_ShouldGetDefaultResponse()
        {
            var fakeRequestMaker = new FakeScheduleRequestMaker();
            var year = fakeRequestMaker.InvalidYear;
            Dictionary<int, int> cache = new Dictionary<int, int>();

            var cut = new NhlScheduleGetter(fakeRequestMaker, cache, A.Fake<ILoggerFactory>());
            List<int> teams = await cut.GetTeamsForSeason(year);

            teams.Count.Should().Be(0);
            teams.Should().NotBeNull();
        }
        [TestMethod]
        public async Task CallToGetTeamsForSeason_WithValidResponse_ShouldGetResponse()
        {
            var fakeRequestMaker = new FakeScheduleRequestMaker();
            var year = fakeRequestMaker.ValidYear;
            Dictionary<int, int> cache = new Dictionary<int, int>();

            var cut = new NhlScheduleGetter(fakeRequestMaker, cache, A.Fake<ILoggerFactory>());
            List<int> teams = await cut.GetTeamsForSeason(year);

            teams.Should().NotBeNull();
            teams.Count.Should().Be(3);
        }
        [TestMethod]
        public async Task CallToGetGameCountInSeason_WithCachedResponse_ShouldGetCachedResponse()
        {
            var fakeRequestMaker = new FakeScheduleRequestMaker();
            var year = fakeRequestMaker.ValidYear;
            var cachedGameCount = 1300;
            Dictionary<int, int> cache = new Dictionary<int, int>();
            cache.Add(year, cachedGameCount);

            var cut = new NhlScheduleGetter(fakeRequestMaker, cache, A.Fake<ILoggerFactory>());
            var count = await cut.GetGameCountInSeason(year);

            count.Should().Be(cachedGameCount);
        }
        [TestMethod]
        public async Task CallToGetGameCountInSeason_WithUncachedInvalidResponse_ShouldGetDefaultResponse()
        {
            var fakeRequestMaker = new FakeScheduleRequestMaker();
            var year = fakeRequestMaker.InvalidYear;
            var defaultGameCount = 1400;
            Dictionary<int, int> cache = new Dictionary<int, int>();

            var cut = new NhlScheduleGetter(fakeRequestMaker, cache, A.Fake<ILoggerFactory>());
            var count = await cut.GetGameCountInSeason(year);

            count.Should().Be(defaultGameCount);
        }
        [TestMethod]
        public async Task CallToGetGameCountInSeason_WithUncachedResponse_ShouldGetUncachedResponse()
        {
            var fakeRequestMaker = new FakeScheduleRequestMaker();
            var year = fakeRequestMaker.ValidYear;
            var expectedGameCount = 200;
            Dictionary<int, int> cache = new Dictionary<int, int>();

            var cut = new NhlScheduleGetter(fakeRequestMaker, cache, A.Fake<ILoggerFactory>());
            var count = await cut.GetGameCountInSeason(year);

            count.Should().Be(expectedGameCount);
        }
    }
}
