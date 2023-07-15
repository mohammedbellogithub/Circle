using Microsoft.Extensions.Caching.Memory;

namespace Circle.Core.Services.Cache
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memorycache;
        public CacheService(IMemoryCache memorycache)
        {
            _memorycache = memorycache;
        }

        public string GetCacheKey(string key)
        {
          var cache = _memorycache.Get<string>(key);
            return cache;
        }

        public void SetCacheInfo(string key, string OTP)
        {
            var expiryTime = new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(3)
            };
            _memorycache.Set(key, OTP, expiryTime);
        }
    }
}
