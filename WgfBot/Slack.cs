using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WgfBot
{
    public class Slack
    {
        public const string GeneralChannel = "CB6CAU25U";
        public const string MemesChannel = "CJRBY6BNH";

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
