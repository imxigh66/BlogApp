using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public class CacheService
    {
        private static readonly Lazy<CacheService> _instance = new Lazy<CacheService>(() => new CacheService());

        private readonly ConcurrentDictionary<string, object> _cache;
        private readonly ConcurrentDictionary<string, DateTime> _expirations;

        public CacheService()
        {
            _cache = new ConcurrentDictionary<string, object>();
            _expirations = new ConcurrentDictionary<string, DateTime>();
        }

        public static CacheService Instance => _instance.Value;

        public void Set<T>(string key,T value,TimeSpan expiration)
        {
            _cache[key] = value;
            _expirations[key] = DateTime.UtcNow.Add(expiration);
        }

        public bool TryGetValue<T>(string key,out T value)
        {
            value = default;

            if (!_cache.TryGetValue(key, out var cachedValue))
                return false;
            if(_expirations.TryGetValue(key,out var expiration) && expiration < DateTime.UtcNow)
            {
                // Кэш устарел, удаляем его
                _cache.TryRemove(key, out _);
                _expirations.TryRemove(key, out _);
                return false;
            }

            value = (T)cachedValue;
            return true;
        }

        public void Remove(string key)
        {
            _cache.TryRemove(key, out _);
            _expirations.TryRemove(key, out _);
        }

        // Метод для получения статистики кэша
        public CacheStats GetStats()
        {
            return new CacheStats
            {
                ItemCount = _cache.Count,
                Keys = _cache.Keys.ToList()
            };
        }
    }
    public class CacheStats
    {
        public int ItemCount { get; set; }
        public List<string> Keys { get; set; }
    }
}
