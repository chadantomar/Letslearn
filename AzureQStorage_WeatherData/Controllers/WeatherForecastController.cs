using Azure.Storage.Queues;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AzureQStorage_WeatherData.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly QueueClient _queueclient;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, QueueClient queueClient)
        {
            _logger = logger;
            _queueclient = queueClient;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpPost]
        public async Task Post([FromBody] WeatherForecast data)
        {
          // create queue client to post data from 
            //swagger UI to Azure Queue 
            if(_queueclient!=null)
            {
                string message = JsonSerializer.Serialize(data);
               var x= await _queueclient.SendMessageAsync(message,null,TimeSpan.FromSeconds(-1)); // this message accept serialize data 
            }

        }
    }
}
