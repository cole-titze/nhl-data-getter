using ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakeLineScoreResponse;

namespace ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakeGameDataResponse
{
	public class FakeLineScore
	{
        public FakeTotals totals { get; set; } = new FakeTotals();
    }
}