using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using FSSwaggerProviderDemo.Common;
using FSSwaggerProviderDemo.MyWeatherApi.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace FSSwaggerProviderDemo.MyWeatherApi
{
    public class GetWeather
    {
        private readonly IOptions<ServiceSettings> _settings;
        private readonly HttpClient _httpClient;

        public GetWeather(IHttpClientFactory httpClientFactory, IOptions<ServiceSettings> settings)
        {
            _settings = settings;
            _httpClient = httpClientFactory.CreateClient();
        }

        [FunctionName("GetWeather")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string city = req.Query["city"];
            
            if(city== null)
                return new BadRequestObjectResult("Please pass a city on the query string");

            var weatherServiceUrl = $"{_settings.Value.WeatherServiceUrl}?city={city}";
            var response = await _httpClient.GetAsync(weatherServiceUrl);

            var weather = await response.Content.ReadAsAsync<Weather>();

            return weather != null
                ? (ActionResult)new OkObjectResult(response)
                : new BadRequestObjectResult("Error ocurred");
            
        }
    }
}