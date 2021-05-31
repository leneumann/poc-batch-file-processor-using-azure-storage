using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worker.Infra.AzureStorage.BlobStorage;
using Worker.Infra.AzureStorage.Queue;
using Worker.Infra.IoC;

namespace Worker
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddOptions<BlobStorageOptions>()
                .Bind(_configuration.GetSection(BlobStorageOptions.BlobStorage));
            services.AddOptions<AzureQueueOptions>()
                .Bind(_configuration.GetSection(AzureQueueOptions.AzureQueue));
            //services.AddHealthChecks()
            //    .AddCheck<ExampleHealthCheck>("example_health_check");
            services.AddHostedService<BlobWorker>();
            services.AddDependencyInjection();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapHealthChecks("/ready", new HealthCheckOptions()
            //    {
            //        Predicate = (check) => check.Tags.Contains("ready"),
            //    });

            //    endpoints.MapHealthChecks("/live", new HealthCheckOptions()
            //    {
            //        Predicate = (_) => false
            //    });
            //});
        }
    }
}
