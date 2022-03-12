using Azure.Identity;
using Azure.Storage.Queues;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AZ_Q_func_WeatherData
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AZ_Q_func_WeatherData", Version = "v1" });
            });
           // services.AddHostedService<bg_WeatherDataService>();// Bg service will start on start of application

            services.AddAzureClients(builder => {
                builder.AddClient<QueueClient, QueueClientOptions>((options,_,_) =>
                 {
                     options.MessageEncoding = QueueMessageEncoding.Base64;
                    // var credential = new DefaultAzureCredential();
                    // var queueuri = new Uri("https://saqsweather.queue.core.windows.net/add-weatherdata");
                     string connectionstring = "DefaultEndpointsProtocol=https;AccountName=saqsweather;AccountKey=4Be64simvP6SQzIGPPzL6HRf6ga63ho4tbTjwqt8SAs9pCm6/Wd7e4o8lqkQcTksHXM36LB9t/Zj6ZbKPeqB7w==;EndpointSuffix=core.windows.net";
                     string queuename = "add-weatherdata";
                     return new QueueClient(connectionstring, queuename, options);
                 });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AZ_Q_func_WeatherData v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
