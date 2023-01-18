using Services.RequestMaker;
using ServicesTests.UnitTests.RequestMakerTests.Fakes;

namespace ServicesTests.UnitTests.NhlData
{
    [TestClass]
    public class RequestMakerTests
    {

        [TestMethod]
        public async Task CallToMakeRequest_WithUnsuccessfulStatusCode_ShouldReturnNull()
        {
            string url = "http://null";
            string query = "invalid";

            var cut = new RequestMaker(new FakeClient());
            dynamic? response = await cut.MakeRequest(url, query);

            Assert.IsNull(response);
        }
        [TestMethod]
        public async Task CallToMakeRequest_WithSuccessfulStatusCode_ShouldAttemptToCreateDynamic()
        {
            string url = "http://null";
            string query = "valid";

            var cut = new RequestMaker(new FakeClient());
            dynamic? response = await cut.MakeRequest(url, query);

            Assert.IsNull(response);
        }
    }
}
