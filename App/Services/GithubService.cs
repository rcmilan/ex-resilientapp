using App.Models;
using Polly;
using Polly.Retry;
using System.Net;
using System.Text.Json;

namespace App.Services
{
    public interface IGithubService
    {
        Task<User> Get(string username);
    }

    public class GithubService : IGithubService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly AsyncRetryPolicy<User> _retryPolicy;

        private static readonly Random rnd = new Random();

        public GithubService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _retryPolicy = Policy<User>.Handle<HttpRequestException>().RetryAsync(retryCount: 3);
        }

        public async Task<User> Get(string username)
        {
            var client = _httpClientFactory.CreateClient("GitHub");

            return await _retryPolicy.ExecuteAsync(async () =>
            {
                if (rnd.Next(1, 3) == 1)
                    throw new HttpRequestException();

                var result = await client.GetAsync($"/users/{username}");
                if (result.StatusCode == HttpStatusCode.NotFound)
                    return null;

                var resultString = await result.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<User>(resultString);
            });
        }
    }
}