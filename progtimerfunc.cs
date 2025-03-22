using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AZ_204
{
    public class progtimerfunc
    {
        private readonly ILogger _logger;

        public progtimerfunc(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<progtimerfunc>();
        }

        [Function("progtimerfunc")]
        public void Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            
            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
