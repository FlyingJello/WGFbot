using System;
using System.Collections.Generic;
using System.Net.Http;
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

            await BroadcastMessage("GKE9MJJE5", "<!here> Posture check");
        }

        public static async Task BroadcastMessage(string channel, string message)
        {
            var token = Environment.GetEnvironmentVariable("BOT_TOKEN");

            var payload = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("token", token),
                new KeyValuePair<string, string>("channel", channel),
                new KeyValuePair<string, string>("text", message)
            };

            using var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://slack.com/api/chat.postMessage") { Content = new FormUrlEncodedContent(payload) };
            await httpClient.SendAsync(request);
        }
    }
}
