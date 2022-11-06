using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Services.RequestMaker
{
	public class RequestMaker : IRequestMaker
	{
        /// <summary>
        /// Given a url and query makes a request and returns the response converted to a dynamic object
        /// </summary>
        /// <param name="url">Base url to call</param>
        /// <param name="query">Query parameters to append to url</param>
        /// <returns>Dynamic response object</returns>
        public async Task<dynamic?> MakeRequest(string url, string query)
        {
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                response = await client.GetAsync(query);
            }
            
            return await ParseResponse(response);
        }
        /// <summary>
        /// Converts the raw http response into a dynamic object.
        /// </summary>
        /// <param name="response">The raw http response message</param>
        /// <returns>Dynamic object</returns>
        private async Task<dynamic?> ParseResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                return null;

            // Get data as Json string 
            string data = await response.Content.ReadAsStringAsync();
            // Add Json string conversion to hard object
            var message = JsonConvert.DeserializeObject<dynamic>(data);

            return message;
        }
    }
}

