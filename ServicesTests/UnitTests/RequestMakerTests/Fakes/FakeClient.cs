using Services.RequestMaker;
using FakeItEasy;

namespace ServicesTests.UnitTests.RequestMakerTests.Fakes
{
    public class FakeClient : IHttpClient
    {
        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            HttpResponseMessage response;
            if (request.RequestUri!.ToString() == "http://nullinvalid/")
            {
                response = new HttpResponseMessage();
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return Task.FromResult(response);
            }

            response = new HttpResponseMessage();
            response.StatusCode=System.Net.HttpStatusCode.OK;
            response.Content = A.Fake<HttpContent>();

            return Task.FromResult( new HttpResponseMessage());
        }
    }
}
