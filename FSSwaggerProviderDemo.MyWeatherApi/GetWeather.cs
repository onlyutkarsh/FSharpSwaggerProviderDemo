using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FSSwaggerProviderDemo.MyWeatherApi
{
    public static class GetWeather
    {
        [FunctionName("GetWeather")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string city = req.Query["city"];

            return city != null
                ? (ActionResult)new OkObjectResult($"Hello, {city}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
            
        }
    }
}