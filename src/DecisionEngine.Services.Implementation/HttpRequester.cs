using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using DecisionEngine.Services.Interfaces;
using DecisionEngine.Models;

namespace DecisionEngine.Services.Implementation
{
    public class HttpRequester : IHttpRequester
    {
        private readonly ApiSettings _apiSettings;
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient = new HttpClient();
        
        public HttpRequester(ApiSettings apiSettings, ILogger<HttpRequester> logger)
        {
            _apiSettings = apiSettings;
            _logger = logger;
        }

        public async Task<Response> PostAsync(User model)
        {
            var result = await _httpClient.PostAsync($"{_apiSettings.Url}decision", ParsePayload(model));

            if (result.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<Response>(await result.Content.ReadAsStringAsync());

            _logger.LogError($"Error: {result.StatusCode.ToString()} Message: {result.ReasonPhrase}");
            return new Response { DecisionResult = Status.Errored.ToString() };
        }

        private HttpContent ParsePayload<T>(T model)
        {
            var byteContent = new ByteArrayContent(
                Encoding.UTF8.GetBytes(
                    JsonConvert.SerializeObject(model)));

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return byteContent;
        }
    }
}
