using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Core.Services.Cache
{
    public interface ICacheService
    {
        string GetCacheKey(string key);
        void SetCacheInfo(string key, string OTP);
    }
}
