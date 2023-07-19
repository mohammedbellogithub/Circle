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
        Task<T> GetAsync<T>(string key);
        Task<bool> SetAsync<T>(string key, T value, int duration);
        Task ClearAsync(string key);
        void ClearAll();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// 


        //void ClearCache(string key);
        //string GetCacheKey(string key);
        //T GetGenericCache<T>(string key);

        //void SetCacheInfo<T>(string key, T OTP, int duration);
    }
}
