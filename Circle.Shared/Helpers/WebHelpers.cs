using Circle.Shared.Security;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Shared.Helpers
{
    public class WebHelpers
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static UserPrincipal CurrentUser
        {
            get
            {
                return new UserPrincipal(_httpContextAccessor.HttpContext.User);
            }
        }
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static HttpContext HttpContext
        {
            get { return _httpContextAccessor.HttpContext; }
        }

        public static string GetRemoteIP
        {
            get { return HttpContext.Connection.RemoteIpAddress.ToString(); }
        }

        public static string GetUserAgent
        {
            get { return HttpContext.Request.Headers["User-Agent"].ToString(); }
        }

        public static string GetScheme
        {
            get { return HttpContext.Request.Scheme; }
        }
    }
}
