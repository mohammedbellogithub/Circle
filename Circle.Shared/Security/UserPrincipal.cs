using Circle.Shared.Helpers;
using Microsoft.IdentityModel.JsonWebTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Shared.Security
{
    public class UserPrincipal : ClaimsPrincipal
    {
        public UserPrincipal(ClaimsPrincipal principal)
            : base(principal)
        {
            
        }
        private string GetClaimValue(string key)
        {
            var claim = this.Claims.FirstOrDefault(c => c.Type == key);
            return claim?.Value;
        }

        public bool IsDefaultPassword
        {
            get
            {
                if (FindFirst(ClaimTypeHelpers.IsDefaultPassword) == null)
                    return false;

                var value = GetClaimValue(ClaimTypeHelpers.IsDefaultPassword);
                bool.TryParse(value, out bool isDefaultPassword);
                return isDefaultPassword;
            }
        }

        public string Role
        {
            get
            {
                if (FindFirst(ClaimTypes.Role) == null)
                    return string.Empty;

                return GetClaimValue(ClaimTypes.Role);
            }
        }

        public string UserName
        {
            get
            {
                if (FindFirst(ClaimTypeHelpers.name) == null)
                    return string.Empty;

                return GetClaimValue(ClaimTypeHelpers.name);
            }
        }

        public string FirstName
        {
            get
            {
                if (FindFirst(ClaimTypeHelpers.FirstName) == null)
                    return string.Empty;

                return GetClaimValue(ClaimTypeHelpers.FirstName).ToString();
            }
        }
        public string LastName
        {
            get
            {
                if (FindFirst(ClaimTypeHelpers.LastName) == null)
                    return string.Empty;

                return GetClaimValue(ClaimTypeHelpers.LastName).ToString();
            }
        }

        public string UserId
        {
            get
            {
                if (FindFirst(JwtRegisteredClaimNames.Sub) == null)
                    return string.Empty;

                return GetClaimValue(JwtRegisteredClaimNames.Sub);
            }
        }

        public string Email
        {
            get
            {
                if (FindFirst(ClaimTypes.Email) == null)
                    return string.Empty;

                return GetClaimValue(ClaimTypes.Email);
            }
        }
    }
}
