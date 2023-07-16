using Circle.Core.Services.Cache;
using Circle.Core.Services.User;
using Circle.Shared.Enums;
using Circle.Shared.Extensions;
using Circle.Shared.Helpers;
using Circle.Shared.Models.UserIdentity;
using Circle.Shared.Security.Permission;
using IdentityModel;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Circle.Api.Controllers
{
    /// <summary>
    /// OpenId Connect Auth Controller
    /// </summary>
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<AppUsers> _userManager;
        private readonly SignInManager<AppUsers> _signInManager;
        private readonly IOptions<IdentityOptions> _identityOptions;
        private readonly ICacheService _cacheService;
        public AuthController(UserManager<AppUsers> userManager, SignInManager<AppUsers> signInManager, IOptions<IdentityOptions> identityOptions, ICacheService cacheService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _identityOptions = identityOptions;
            _cacheService = cacheService;
        }
        [AllowAnonymous]
        [HttpPost("~/api/auth/token"), Produces("application/json")]
        public async Task<IActionResult> Exchange()
        {
            var request = HttpContext.GetOpenIddictServerRequest();
            if (request == null)
                throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            if (request.IsPasswordGrantType())
            {
                var result = await PasswordSignIn(request);
                return result;
            }
            else if (request.IsRefreshTokenGrantType())
            {
                var result = await RefreshToken(request);
                return result;
            }

            return Error("The specified grant type is not supported.");
        }

        private async Task<IActionResult> RefreshToken(OpenIddictRequest request)
        {
            // Retrieve the claims principal stored in the refresh token.
            var info = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            // Retrieve the user profile corresponding to the refresh token.
            // Note: if you want to automatically invalidate the refresh token
            // when the user password/roles change, use the following line instead:
            // var user = _signInManager.ValidateSecurityStampAsync(info.Principal);
            var user = await _userManager.GetUserAsync(info.Principal);

            if (user == null)
            {
                return Error("The refresh token is no longer valid.");
            }

            // Ensure the user is still allowed to sign in.
            if (!await _signInManager.CanSignInAsync(user))
            {
                return Error("The user is no longer allowed to sign in.");
            }

            // Create a new authentication ticket, but reuse the properties stored
            // in the refresh token, including the scopes originally granted.
            var principal = await CreateTicketAsync(request, user, info.Properties);
            var signInResult = SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            return signInResult;
        }

        private async Task<IActionResult> PasswordSignIn(OpenIddictRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Username);

            //check if user is available
            if (user == null || user.IsDeleted)
            {
                return Error("Email or password is incorrect.");
            }

            // Check if profile is activated
            if (!user.Activated)
            {
                return Error("Your profile has not been activated.");
            }

            // Ensure the user is allowed to sign in.
            if (!await _signInManager.CanSignInAsync(user))
            {
                return Error("You are not allowed to sign in.");

            }

            // Reject the token request if two - factor authentication has been enabled by the user.
            //if (_userManager.SupportsUserTwoFactor && await _userManager.GetTwoFactorEnabledAsync(user))
            //{

            //Check User Password
            var checkUserPassword = await _signInManager.PasswordSignInAsync(user, request.Password, false ,lockoutOnFailure: true);

            if (checkUserPassword.IsLockedOut)
            {
                return Error("Your account is temporarily locked out. Please try again later.");
            }

            if (!checkUserPassword.Succeeded)
            {
                return Error("Username or password is incorrect.");
              
            }

            // Create a new authentication ticket.
            var principal = await CreateTicketAsync(request, user);

            var signInResult = SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            
            return signInResult;
        }

        private async Task<ClaimsPrincipal> CreateTicketAsync(OpenIddictRequest oidcRequest, AppUsers user,
          AuthenticationProperties? properties = null)
        {
            var principal = await _signInManager.CreateUserPrincipalAsync(user);
            var identity = principal.Identity as ClaimsIdentity;

            var permissionClaims = (from c in principal.Claims
                                    where c.Type == nameof(Permission)
                                    select c).ToArray();


            Array.ForEach(permissionClaims, identity.RemoveClaim);
            await CacheUserPermission(user.Id.ToString(), permissionClaims);

            await AddClaims(principal, user, oidcRequest);

            // Create a new authentication ticket holding the user identity.

            if (!oidcRequest.IsRefreshTokenGrantType())
            {
                // Set the list of scopes granted to the client application.
                // Note: the offline_access scope must be granted
                // to allow OpenIddict to return a refresh token.
                principal.SetScopes(new[]
                {
                   Scopes.OpenId,
                   Scopes.Email,
                   Scopes.Profile,
                   Scopes.OfflineAccess,
                   Scopes.Roles,
                }.Intersect(oidcRequest.GetScopes()));
            }

            principal.SetResources("resource_server");

            
            foreach (var claim in principal.Claims)
            {
                claim.SetDestinations(GetDestinations(claim, principal));
            }

            return principal;
        }

        /// <summary>
        ///  Customized error that is returned in case of authentication error
        /// </summary>
        protected virtual IActionResult Error(string description)
        {
            var properties = new AuthenticationProperties(
                new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = description
                }!
            );

            return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Sets an array of claims for the specified user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="permissionClaims"></param>
        /// <returns></returns>
        
        private async Task CacheUserPermission(string userId, Claim[] permissionClaims)
        {
            //clears memory cache allocated for specified user
             _cacheService.ClearCache(userId);

            var permissions = from pc in permissionClaims
                              select new PermissionProperties
                              {
                                  Name = pc.Type,
                                  Id = pc.Value
                              };

             _cacheService.SetCacheInfo(userId, permissions, 300);
        }

        /// <summary>
        /// Adds claims from <param name="user"/> to <param name="principal"/>.
        /// Override this function if you want to remove/modify some pre-added claims.
        /// If you just want to add more claims, consider overriding <see cref="GetClaims"/>
        /// </summary>
        protected virtual async Task AddClaims(ClaimsPrincipal principal, AppUsers user, OpenIddictRequest openIddictRequest)
        {
            IList<Claim> claims = await GetClaims(user, openIddictRequest);

            ClaimsIdentity claimIdentity = principal.Identities.First();
            claimIdentity.AddClaims(claims);
        }

        /// <summary>
        /// Returns claims that will be added to the user's principal (and later to JWT token).
        /// Consider overriding this function if you want to add more claims.
        /// </summary>
        protected virtual Task<IList<Claim>> GetClaims(AppUsers user, OpenIddictRequest openIddictRequest)
        {
            return Task.FromResult(new List<Claim>()
                {
                new(JwtClaimTypes.NickName, user.UserName),
                new(JwtClaimTypes.Id, user.Id.ToString() ?? string.Empty),
                new(JwtClaimTypes.Subject, user.Id.ToString() ?? string.Empty),
                } as IList<Claim>
            ) ;
        }

        /// <summary>
        /// Returns destinations to which a certain claim could be returned
        /// </summary>
        protected virtual IEnumerable<string> GetDestinations(Claim claim, ClaimsPrincipal principal)
        {
            // Note: by default, claims are NOT automatically included in the access and identity tokens.
            // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
            // whether they should be included in access tokens, in identity tokens or in both.
            switch (claim.Type)
            {
                case Claims.Name:
                    yield return Destinations.AccessToken;

                    if (principal.HasScope(Scopes.Profile))
                        yield return Destinations.IdentityToken;

                    yield break;

                case Claims.Email:
                    yield return Destinations.AccessToken;

                    if (principal.HasScope(Scopes.Email))
                        yield return Destinations.IdentityToken;

                    yield break;

                case Claims.Role:
                    yield return Destinations.AccessToken;

                    if (principal.HasScope(Scopes.Roles))
                        yield return Destinations.IdentityToken;

                    yield break;

                // Never include the security stamp in the access and identity tokens, as it's a secret value.
                case "AspNet.Identity.SecurityStamp":
                    yield break;

                default:
                    yield return Destinations.AccessToken;
                    yield break;
            }
        }
    }
}
