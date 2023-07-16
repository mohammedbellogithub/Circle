using Circle.Shared.Security.Permission;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace Circle.Core.Services.Cache
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memorycache;
        public CacheService(IMemoryCache memorycache)
        {
            _memorycache = memorycache;
        }

        public  void ClearCache(string key)
        {
             _memorycache.Remove(key);
        }

        public T GetGenericCache<T>(string key)
        {
            T result =  (T) _memorycache.Get(key);
            if (result is null)
            {
                return default;
            }
            return result;
        }

        public string GetCacheKey(string key)
        {
          var cache = _memorycache.Get<string>(key);
            return cache;
        }

        public void SetCacheInfo<T>(string key, T OTP , int duration)
        {
            var expiryTime = new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(duration)
            };
            _memorycache.Set(key, OTP, expiryTime);
        }
    }
}
