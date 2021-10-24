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
        private static readonly HttpClient _httpClient = new() { BaseAddress = new Uri(ApiAddress) };

        private static async Task Main(string[] args)
        {
            var disco = await _httpClient.GetDiscoveryDocumentAsync(IdentityAddress);
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                Console.ReadKey();
                return;
            }

            var passwordRequestToken = await PasswordRequestTokenAsync(disco);
            var deviceRequestToken = await DeviceRequestTokenAsync(disco);
            
            _httpClient.SetBearerToken(passwordRequestToken);

            var response = await _httpClient.GetAsync("api/WeatherForecast");
            var responseContent = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseContent);
            Console.ReadKey();
        }

        private static async Task<string> PasswordRequestTokenAsync(DiscoveryDocumentResponse disco)
        {
            var passwordTokenRequest = new PasswordTokenRequest()
            {
                Address = disco.TokenEndpoint,
                ClientId = "client1",
                UserName = "TestUser1",
                Password = "Password1#",
                Scope = "api.client"
            };

            var passwordResponse = await _httpClient.RequestPasswordTokenAsync(passwordTokenRequest);

            if (passwordResponse.IsError)
            {
                Console.WriteLine(passwordResponse.Error);
                return string.Empty;
            }
            
            Console.WriteLine("Password response:");
            Console.WriteLine(passwordResponse.Json);
            Console.WriteLine("\n");

            return passwordResponse.AccessToken;
        }

        private static async Task<string> DeviceRequestTokenAsync(DiscoveryDocumentResponse disco)
        {
            var deviceAuthRequest = new DeviceAuthorizationRequest()
            {
                Address = disco.DeviceAuthorizationEndpoint,
                ClientId = "client4",
                Scope = "api.client"
            };
            var deviceAuthResponse = await _httpClient.RequestDeviceAuthorizationAsync(deviceAuthRequest);
            
            Console.WriteLine("Device authorization response:");
            Console.WriteLine(deviceAuthResponse.Json);
            
            if (deviceAuthResponse.IsError)
            {
                Console.WriteLine(deviceAuthResponse.Error);
                return string.Empty;
            }

            var deviceTokenRequest = new DeviceTokenRequest()
            {
                DeviceCode = deviceAuthResponse.DeviceCode
            };
            var deviceResponse = await _httpClient.RequestDeviceTokenAsync(deviceTokenRequest);
            
            if (deviceResponse.IsError)
            {
                Console.WriteLine(deviceResponse.Error);
                return string.Empty;
            }
            
            Console.WriteLine("Device response:");
            Console.WriteLine(deviceResponse.Json);
            Console.WriteLine("\n");

            return deviceResponse.AccessToken;
        }
    }
}
