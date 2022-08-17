using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Common
{
    public class CapstoneMemoryCache
    {
        private static readonly CapstoneMemoryCache instance = new CapstoneMemoryCache();
        private MemoryCache _cache;
        
        static CapstoneMemoryCache()
        {
        }
        private CapstoneMemoryCache()
        {
            _cache = new MemoryCache(new MemoryCacheOptions
            {
                ExpirationScanFrequency = TimeSpan.FromHours(1)
            });
        }

        public static CapstoneMemoryCache Instance
        {
            get
            {
                return instance;
            }
        }
        /// <summary>
        /// Get cache value by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetValue<T>(string key)
        {
            return (T)_cache.Get(key);
        }

        /// <summary>
        /// Add a cache object with date expiration
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheTime">Minute</param>
        /// <returns></returns>
        public T Add<T>(string key, object value, int cacheTime)
        {
            var expiredDate = DateTime.UtcNow + TimeSpan.FromMinutes(cacheTime);
            return (T)_cache.Set(key, value, expiredDate);
        }

        /// <summary>
        /// Delete cache value from key
        /// </summary>
        /// <param name="key"></param>
        public void Delete(string key)
        {
            if (Exists(key))
            {
                _cache.Remove(key);
            }
        }

        public bool Exists(string key)
        {
            bool result = false;
            if (_cache.TryGetValue(key, out var _))
            {
                result = true;
            }
            return result;
        }

    }
}
