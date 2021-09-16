using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using FSSwaggerProviderDemo.Common;
using FSSwaggerProviderDemo.MyWeatherApi.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
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

        [OpenApiOperation(operationId: "get-weather", Description = "Get the weater information for the provided city")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "city", In = ParameterLocation.Path, Required = true, Type = typeof(string), Summary = "The name of the city")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: MediaTypeNames.Application.Json, bodyType: typeof(Weather), Summary = "Weather Information", Description = "This returns the weather information")]
        [FunctionName("GetWeather")]
        public async Task<IActionResult> GetWeatherAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "weather/{city}")] HttpRequest req, ILogger log, string city)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if(city== null)
                return new BadRequestObjectResult("Please pass a city on the query string");

            var weatherServiceUrl = $"{_settings.Value.WeatherServiceUrl}?city={city}";
            var response = await _httpClient.GetAsync(weatherServiceUrl);

            var weather = await response.Content.ReadAsAsync<Weather>();

            return weather != null
                ? (ActionResult)new OkObjectResult(weather)
                : new BadRequestObjectResult("Error ocurred");
            
        }
    }
}