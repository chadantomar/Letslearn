using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AZ_Q_weatherinjest
{
    public static class Function1
    {
        [FunctionName("ProcessWeatherData")]
        public static void Run([QueueTrigger("add-weatherdata", Connection = "weatherdataqueue")]string myQueueItem, ILogger log)
        {
            if (myQueueItem.Contains("exception")) throw new Exception();
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
