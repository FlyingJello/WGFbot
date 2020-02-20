using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

        public static async Task BroadcastImage(string channel, string image)
        {
            var token = Environment.GetEnvironmentVariable("BOT_TOKEN");

            var attachmentString = JsonConvert.SerializeObject(new List<Attachment> { new Attachment { image_url = image, fallback = "meme"} });

            var payload = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("token", token),
                new KeyValuePair<string, string>("channel", channel),
                new KeyValuePair<string, string>("attachments", attachmentString)
            };

            using var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://slack.com/api/chat.postMessage") { Content = new FormUrlEncodedContent(payload) };
            await httpClient.SendAsync(request);
        }
    }

    public class Attachment {
        public string fallback { get; set; }
        public string image_url { get; set; }
    }
}
