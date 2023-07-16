using Circle.Core.Services.Cache;
using Circle.Shared.Helpers;
using Circle.Shared.Security.Permission;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Core.Components.Policy
{
    public class PermissionsAuthorizationHandler : AuthorizationHandler<PermissionsAuthorizationRequirement>
    {
        private readonly ICacheService _cacheService;

        public PermissionsAuthorizationHandler(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionsAuthorizationRequirement requirement)
        {
            var sysAdmin = context.User.IsInRole(RoleHelpers.SYS_ADMIN);

            if (sysAdmin)
                context.Succeed(requirement);

            var userId = context.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var cachedPermission =  _cacheService.GetGenericCache<IEnumerable<PermissionProperties>>(key: userId);


            if (cachedPermission != null)
            {
                var currentUserPermissions = cachedPermission.Select(cp => cp.Id).ToList();

                var authorized = requirement.RequiredPermissions.AsParallel()
                    .Any(rp => currentUserPermissions.Contains(rp.ToString()));

                if (authorized)
                    context.Succeed(requirement);
            }

        }
    }
}
