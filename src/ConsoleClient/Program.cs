using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace ConsoleClient
{
    internal class Program
    {
        private const string ApiAddress = "https://localhost:5001";
        private const string IdentityAddress = "https://localhost:5002";
        private static readonly HttpClient _apiClient = new HttpClient { BaseAddress = new Uri(ApiAddress) };

        private static async Task Main(string[] args)
        {
            var disco = await _apiClient.GetDiscoveryDocumentAsync(IdentityAddress);
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                Console.ReadKey();
                return;
            }

            var tokenClient = new TokenClient(_apiClient, new TokenClientOptions()
            {
                Address = disco.TokenEndpoint,
                ClientId = "client1"
            });

            var tokenResponse = await tokenClient.RequestPasswordTokenAsync("TestUser1", "Password1#", "api.client");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                Console.ReadKey();
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            _apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await _apiClient.GetAsync("api/WeatherForecast");
            var responseContent = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseContent);
            Console.WriteLine("\n\n");
            Console.ReadKey();
        }
    }
}
