using Circle.Shared.Security.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Core.Services.Cache
{
    public interface ICacheService
    {
        void ClearCache(string key);
        string GetCacheKey(string key);
        T GetGenericCache<T>(string key);

        void SetCacheInfo<T>(string key, T OTP, int duration);
    }
}
