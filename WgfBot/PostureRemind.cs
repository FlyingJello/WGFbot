using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace WgfBot
{
    public static class PostureRemind
    {
        [FunctionName("PostureRemind")]
        public static async Task Run([TimerTrigger("0 14 14-22 * * MON-FRI")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"PostureRemind executed at: {DateTime.Now}");


            if (new Random().Next(4) == 0)
            {
                await Slack.BroadcastMessage(Slack.GeneralChannel, "<!here> Posture check");
            }
        }
    }
}
