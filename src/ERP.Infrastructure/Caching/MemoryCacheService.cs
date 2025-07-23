using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace ERP.Infrastructure.Caching
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly ConcurrentDictionary<string, bool> _cacheKeys;

        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
            _cacheKeys = new ConcurrentDictionary<string, bool>();
        }

        public Task<T?> GetAsync<T>(string key)
        {
            _cache.TryGetValue(key, out T? value);
            return Task.FromResult(value);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var options = new MemoryCacheEntryOptions();

            if (expiry.HasValue)
            {
                options.AbsoluteExpirationRelativeToNow = expiry.Value;
            }
            else
            {
                options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
            }

            options.RegisterPostEvictionCallback((evictedKey, evictedValue, reason, state) =>
            {
                _cacheKeys.TryRemove(evictedKey.ToString()!, out _);
            });

            _cache.Set(key, value, options);
            _cacheKeys.TryAdd(key, true);

            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key)
        {
            _cache.Remove(key);
            _cacheKeys.TryRemove(key, out _);
            return Task.CompletedTask;
        }

        public Task RemoveByPatternAsync(string pattern)
        {
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            var keysToRemove = _cacheKeys.Keys.Where(k => regex.IsMatch(k)).ToList();

            foreach (var key in keysToRemove)
            {
                _cache.Remove(key);
                _cacheKeys.TryRemove(key, out _);
            }

            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(string key)
        {
            return Task.FromResult(_cacheKeys.ContainsKey(key));
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> getItem, TimeSpan? expiry = null)
        {
            var cachedValue = await GetAsync<T>(key);

            if (cachedValue != null)
            {
                return cachedValue;
            }

            var item = await getItem();
            await SetAsync(key, item, expiry);

            return item;
        }
    }
}