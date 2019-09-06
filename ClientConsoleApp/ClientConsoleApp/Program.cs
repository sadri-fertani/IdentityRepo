using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using ClientConsoleApp.Models;
using System.Net;

namespace ClientConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Any())
            {
                Console.WriteLine();

                foreach (var arg in args)
                {
                    var result = CallWebApi(arg).GetAwaiter().GetResult();

                    Console.WriteLine($"Argument : {arg}");
                    Console.WriteLine("------------------");
                    Console.WriteLine($"StatusCode : {result.StatusCode}");
                    if (result.Result != null) Console.WriteLine(result.Result);
                    Console.WriteLine();
                }

                Console.WriteLine();
            }
            else
                throw new ArgumentException("No arguments.");
        }

        public static async Task<ResultModel> CallWebApi(string arg)
        {
            if (string.IsNullOrEmpty(arg))
                throw new ArgumentException("Argument is null or empty.");

            // Discover
            var discoveryClient = new DiscoveryClient("http://homeserver/identityserver");
            discoveryClient.Policy.RequireHttps = false;
            discoveryClient.Policy.ValidateIssuerName = false;

            var discoveryResponse = await discoveryClient.GetAsync();

            if (discoveryResponse.IsError)
                throw new HttpRequestException(discoveryResponse.Error);

            // Get Token
            var tokenClient = new TokenClient(discoveryResponse.TokenEndpoint, "ClientConsoleApp", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api.scope");

            if (tokenResponse.IsError)
                throw new HttpRequestException(tokenResponse.Error);

            // Inject Token
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);
            client.DefaultRequestHeaders.Add("X-Version", "1.0");

            HttpResponseMessage response;

            switch (arg)
            {
                case "pays":
                    response = await client.GetAsync("http://homeserver/HomeAPI/api/Pays/219");
                    break;
                case "camps":
                    response = await client.GetAsync("http://homeserver/HomeAPI/api/Camps?includeTalks=false");
                    break;
                default:
                    throw new ArgumentException("Argument invalid.");
            }

            if (!response.IsSuccessStatusCode)
                return new ResultModel
                {
                    StatusCode = response.StatusCode,
                    Result = null
                };
            else
            {
                try
                {
                    try
                    {
                        return new ResultModel
                        {
                            StatusCode = response.StatusCode,
                            Result = JArray.Parse(await response.Content.ReadAsStringAsync())
                        };
                    }
                    catch
                    {
                        return new ResultModel
                        {
                            StatusCode = response.StatusCode,
                            Result = JObject.Parse(await response.Content.ReadAsStringAsync())
                        };
                    }
                }
                catch
                {
                    return new ResultModel
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        Result = null
                    };
                }
            }
        }
    }
}
