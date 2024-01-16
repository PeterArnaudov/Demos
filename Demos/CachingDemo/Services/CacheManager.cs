using Microsoft.Extensions.Caching.Memory;

namespace CachingDemo.Services
{
    public class CacheManager
    {
        private readonly IMemoryCache cache;

        public CacheManager(
            IMemoryCache cache)
        {
            this.cache = cache;
        }

        public T? Get<T>(
            string key)
        {
            if (cache.TryGetValue<T>(key, out var value))
            {
                return value;
            }

            return default;
        }

        public T? Get<T>(
            string key,
            Func<T> acquire)
        {
            if (cache.TryGetValue<T>(key, out var value))
            {
                return value;
            }

            // If value couldn't be retrieved from cache,
            // execute the passed function to acquire the value.
            value = acquire();

            if (value != null)
            {
                Set(key, value);
            }

            return default;
        }

        public void Set<T>(
            string key,
            T value,
            int cacheDuration = 60,
            int slidingDuration = 20)
        {
            var cacheOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.UtcNow.AddSeconds(cacheDuration),
                SlidingExpiration = new TimeSpan(0, 0, slidingDuration)
            };

            cache.Set(
                key, value, cacheOptions);
        }

        public void Remove(
            string key)
        {
            cache.Remove(key);
        }
    }
}
