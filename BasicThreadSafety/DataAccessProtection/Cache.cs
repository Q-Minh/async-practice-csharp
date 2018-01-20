using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DataAccessProtection
{
    public class Cache
    {
        private readonly List<NewsItem> items = new List<NewsItem>();

        ReaderWriterLockSlim guard = new ReaderWriterLockSlim();

        public IEnumerable<NewsItem> GetNews(string tag)
        {
            guard.EnterReadLock();
            try
            {
                return items.Where(ni => ni.Tags.Contains(tag)).ToList();
            }
            finally
            {
                guard.ExitReadLock();
            }
        }

        public void AddNewsItem(NewsItem item)
        {
            guard.EnterWriteLock();
            try
            {
                items.Add(item);
            }
            finally
            {
                guard.ExitWriteLock();
            }
        }
    }

    public class NewsItem
    {
        public List<string> Tags { get; set; }
    }
}
