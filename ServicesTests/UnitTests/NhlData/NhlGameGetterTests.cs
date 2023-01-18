using FluentAssertions;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using Services.NhlData;
using ServicesTests.UnitTests.NhlData.Fakes;

namespace ServicesTests.UnitTests.NhlData
{
    [TestClass]
    public class NhlGameGetterTests
    {
        int NullGameId = 2021020001;
        int InvalidGameId = 2021020002;
        int ValidGameId = 2021020100;
        [TestMethod]
        public void CallToGetGame_WithNullGameResponse_ShouldReturnInvalidGame()
        {
            var fakeRequestMaker = new FakeGameRequestMaker();

            var cut = new NhlGameGetter(fakeRequestMaker, A.Fake<ILoggerFactory>());
            var game = cut.GetGame(NullGameId).Result;

            game.IsValid().Should().Be(false);
        }
        [TestMethod]
        public void CallToGetGame_WithInvalidGameResponse_ShouldReturnInvalidGame()
        {
            var fakeRequestMaker = new FakeGameRequestMaker();

            var cut = new NhlGameGetter(fakeRequestMaker, A.Fake<ILoggerFactory>());
            var game = cut.GetGame(InvalidGameId).Result;

            game.IsValid().Should().Be(false);
        }
        [TestMethod]
        public void CallToGetGame_WithValidGameResponse_ShouldReturnValidGame()
        {
            var fakeRequestMaker = new FakeGameRequestMaker();

            var cut = new NhlGameGetter(fakeRequestMaker, A.Fake<ILoggerFactory>());
            var game = cut.GetGame(ValidGameId).Result;

            game.IsValid().Should().Be(true);
        }
    }
}
