using System;
using System.Globalization;
using System.Threading.Tasks;
using Bogus;
using FSSwaggerProviderDemo.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FSSwaggerProviderDemo.WeatherService
{
    public static class PublishWeather
    {
        [FunctionName("PublishWeather")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            TextInfo textInfo = new CultureInfo("en-GB",false).TextInfo;
            
            string city = req.Query["city"];

            var fakeWeather = new Faker<Weather>()
                .RuleFor(x => x.City, f => textInfo.ToTitleCase(city))
                .RuleFor(x => x.High, f => Math.Round(f.Random.Double(18.0, 44.0), 2))
                .RuleFor(x => x.Low, f => Math.Round(f.Random.Double(-4.0, 15.0), 2));

            var weather = fakeWeather.Generate(1);

            return city != null
                ? (ActionResult)new OkObjectResult(weather)
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
            
        }
    }
}