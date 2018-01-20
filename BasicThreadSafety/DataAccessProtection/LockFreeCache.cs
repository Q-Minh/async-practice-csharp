using System.Collections.Generic;
using System.Linq;

namespace DataAccessProtection
{
    public class LockFreeCache
    {
        private List<NewsItem> items = new List<NewsItem>();

        readonly object writeGuard = new object();

        public IEnumerable<NewsItem> GetNews(string tag)
        {
            return items.Where(ni => ni.Tags.Contains(tag));
        }

        public void AddNewsItem(NewsItem item)
        {
            lock (writeGuard)
            {
                var copy = new List<NewsItem>(items);
                copy.Add(item);
                items = copy;
            }
        }
    }
}
