using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using TinyUrl.AuthenticationService.Infrastructure.Clients;
using TinyUrl.AuthenticationService.Infrastructure.Contracts.Options;

namespace TinyUrl.AuthenticationService.Data.Clients
{
    public class UrlClient : IUrlClient
    {
        private readonly HttpClient _httpClient;
        private readonly GenerationServiceOptions _options;

        public UrlClient(HttpClient httpClient, IOptions<GenerationServiceOptions> options)
        {
            _options = options.Value;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_options.BaseUrl!);
        }

        public async Task<int> GetUserUrlCountAsync(string token)
        {
            SetAuthenticationHeader(token);
            var request = await _httpClient.GetAsync("/api/v1/manage/urls");

            if (request.IsSuccessStatusCode)
            {
                var jsonResponse = await request.Content.ReadAsStringAsync();

                var jsonArray = JArray.Parse(jsonResponse);

                return jsonArray.Count;
            }
            else
            {
                throw new Exception();
            }
        }

        private void SetAuthenticationHeader(string token)
        {
            if (_httpClient.DefaultRequestHeaders.Authorization is null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}
