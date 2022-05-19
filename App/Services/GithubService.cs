using App.Helpers;
using App.Models;
using Polly;
using Polly.Retry;
using System.Net;

namespace App.Services
{
    public class GithubService : IGithubService
    {
        private static readonly Random rnd = new();

        private readonly IHttpClientFactory _httpClientFactory;

        private readonly AsyncRetryPolicy _retryPolicy;

        public GithubService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _retryPolicy = Policy
                .Handle<HttpRequestException>(exception => exception.Message != "fake message!")
                .WaitAndRetryAsync(retryCount: 3, attempts => TimeSpan.FromSeconds(1 * attempts));
        }

        public async Task<User> GetUser(string username)
        {
            var client = _httpClientFactory.CreateClient("GitHub");

            return await _retryPolicy.ExecuteAsync(async () =>
            {
                if (rnd.Next(1, 3) == 1) // apenas para simular erros
                    throw new HttpRequestException();

                var result = await client.GetAsync($"/users/{username}");
                if (result.StatusCode == HttpStatusCode.NotFound)
                    return null;

                return result.DeserializeContent<User>();
            });
        }

        public async Task<List<User>> GetUsers(string orgName)
        {
            var client = _httpClientFactory.CreateClient("GitHub");

            return await _retryPolicy.ExecuteAsync(async () =>
            {
                if (rnd.Next(1, 3) == 1) // apenas para simular erros
                    throw new HttpRequestException("fake message!");

                var result = await client.GetAsync($"/orgs/{orgName}");
                if (result.StatusCode == HttpStatusCode.NotFound)
                    return null;

                return result.DeserializeContent<List<User>>();
            });
        }
    }
}