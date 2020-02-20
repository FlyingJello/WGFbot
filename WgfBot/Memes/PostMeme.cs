using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using WgfBot.Memes;

namespace WgfBot
{
    public static class PostMeme
    {
        [FunctionName("PostMeme")]
        public static async Task Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger logger)
        {
            var client = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("AzureWebJobsStorage")).CreateCloudTableClient();
            var table = client.GetTableReference("BlackListedPosts");
            List<string> freshMemes;

            do
            { 
                var sweetMemes = await GetSweetMemes();
                freshMemes = FilterFreshMemes(sweetMemes, table);
            } 
            while (!freshMemes.Any());

            var freshlyHandpickedMeme = freshMemes.First();

            await BlacklistNewMeme(freshlyHandpickedMeme, table);
            await Slack.BroadcastMessage(Slack.MemesChannel, $"https://i.redd.it/{freshlyHandpickedMeme}");
        }

        public static List<string> FilterFreshMemes(List<string> memes, CloudTable table)
        {
            var query = new TableQuery<BlacklistedMeme>();
            query.Where(string.Join($" {TableOperators.Or} ", memes.Select(post => TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, post))));
            var blacklistedMemes = table.ExecuteQuery(query).Select(meme => meme.RowKey).ToList();
            return memes.Except(blacklistedMemes).ToList();
        }

        private static async Task BlacklistNewMeme(string meme, CloudTable table)
        {
            await table.ExecuteAsync(TableOperation.InsertOrMerge(new BlacklistedMeme(meme)));
        }

        private static async Task<List<string>> GetSweetMemes()
        {
            using var httpClient = new HttpClient();
            var result = await httpClient.GetAsync("https://www.reddit.com/r/dankmemes/hot.json?limit=25");
            var redditResponse = JsonConvert.DeserializeObject<RedditResponse>(await result.Content.ReadAsStringAsync());

            return redditResponse.GetImagePosts();
        }
    }
}
