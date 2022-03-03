using Azure.Storage.Queues;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AzureQStorage_WeatherData
{
    public class bg_WeatherDataService : BackgroundService
    {
        private readonly ILogger<bg_WeatherDataService> _logger;
        private readonly QueueClient _qclient;

        public bg_WeatherDataService(ILogger<bg_WeatherDataService> logger,QueueClient queueClient)
        {
            _logger = logger;
            _qclient = queueClient;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
           
            while(!stoppingToken.IsCancellationRequested) // till this service running in background it will pool
                                                          // to Queue and fetch message
            {
                var qMessage = await _qclient.ReceiveMessageAsync(); // get top message
                _logger.LogInformation("Message received from Queue");
                if (qMessage.Value!=null)
                {
                    string datastream = Encoding.ASCII.GetString(qMessage.Value.Body);
                    var data = JsonSerializer.Deserialize<WeatherForecast>(datastream);
                    _logger.LogInformation("Queue data : " + data);
                    // this will delete message once read
                    await _qclient.DeleteMessageAsync(qMessage.Value.MessageId, qMessage.Value.PopReceipt);
                }
            }
            await Task.Delay(TimeSpan.FromSeconds(10));
        }
    }
}
