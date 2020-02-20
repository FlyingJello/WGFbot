using System.Collections.Generic;
using System.Linq;

namespace WgfBot.Memes
{
    public class RedditResponse
    {
        public PostData data { get; set; }

        public List<string> GetImagePosts() => data.children.Where(post => post.data.post_hint == "image").Select(post => post.data.url.Substring(18)).ToList();
    }

    public class PostData
    {
        public List<PostWrapper> children { get; set; }
    }

    public class PostWrapper
    {
        public Post data { get; set; }
    }

    public class Post
    {
        public string post_hint { get; set; }
        public string url { get; set; }
    }
}
