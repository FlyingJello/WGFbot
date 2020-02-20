using Microsoft.Azure.Cosmos.Table;

namespace WgfBot.Memes
{
    class BlacklistedMeme : TableEntity
    {
        public BlacklistedMeme()
        {
        }

        public BlacklistedMeme(string url) : base(url, url)
        {
        }
    }
}
