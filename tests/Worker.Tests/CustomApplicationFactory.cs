using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Worker.App;
using Worker.Infra;

namespace Worker.Tests
{
    public class CustomApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IPublisher, InMemoryCommandProducer>();

            });
            builder.ConfigureAppConfiguration(config => config.AddConfiguration(GetCustomConfiguration()));

        }

        private IConfiguration GetCustomConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.tests.json")
                .Build();

            return config;
        }
    }
}
