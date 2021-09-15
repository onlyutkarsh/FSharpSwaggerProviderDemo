
using System.IO;
using FSSwaggerProviderDemo.MyWeatherApi.Config;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(FSSwaggerProviderDemo.MyWeatherApi.Startup))]
namespace FSSwaggerProviderDemo.MyWeatherApi
{
    public class Startup : FunctionsStartup
    {
        
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var context = builder.GetContext();
            builder.ConfigurationBuilder
                .AddJsonFile(path: Path.Combine(context.ApplicationRootPath ?? "", "appsettings.json"))
                .AddEnvironmentVariables();
        }
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();
            builder.Services.AddOptions<ServiceSettings>().Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection(typeof(ServiceSettings).Name).Bind(settings);
            });
        }
    }
}