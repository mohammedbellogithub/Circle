using Circle.Core.Services.User;
using Circle.Shared.Extensions;
using Circle.Shared.Helpers;
using Circle.Shared.Models.UserIdentity;
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
        public AuthController(UserManager<AppUsers> userManager, SignInManager<AppUsers> signInManager, IOptions<IdentityOptions> identityOptions)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _identityOptions = identityOptions;
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

            user.LastLoginDate = DateTime.Now;
            await _userManager.UpdateAsync(user);

            // Create a new authentication ticket.
            var principal = await CreateTicketAsync(request, user);

            var signInResult = SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            
            return signInResult;
        }

        private async Task<ClaimsPrincipal> CreateTicketAsync(OpenIddictRequest oidcRequest, AppUsers user,
          AuthenticationProperties? properties = null)
        {
            var principal = await _signInManager.CreateUserPrincipalAsync(user);
            var identity = (ClaimsIdentity)principal.Identity;

            AddUserClaims(user, identity);

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

            // Note: by default, claims are NOT automatically included in the access and identity tokens.
            // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
            // whether they should be included in access tokens, in identity tokens or in both.
            var destinations = new List<string>
            {
                Destinations.AccessToken
            };

            foreach (var claim in principal.Claims)
            {
                // Never include the security stamp in the access and identity tokens, as it's a secret value.
                if (claim.Type == _identityOptions.Value.ClaimsIdentity.SecurityStampClaimType)
                {
                    continue;
                }

                // Only add the iterated claim to the id_token if the corresponding scope was granted to the client application.
                // The other claims will only be added to the access_token, which is encrypted when using the default format.
                if ((claim.Type == Claims.Name && principal.HasScope(Scopes.Profile)) ||
                    (claim.Type == Claims.Email && principal.HasScope(Scopes.Email)) ||
                    (claim.Type == Claims.Role && principal.HasScope(Claims.Role)) ||
                    (claim.Type == Claims.Audience && principal.HasScope(Claims.Audience)))
                {
                    destinations.Add(Destinations.IdentityToken);
                }

                claim.SetDestinations(destinations);
            }

            if (string.IsNullOrWhiteSpace(user.FirstName))
            {
                var name = new Claim(Claims.GivenName, user.FirstName ?? "[NA]");
                name.SetDestinations(Destinations.AccessToken, Destinations.IdentityToken);
                identity.AddClaim(name);
            }

            //if (user.IsPasswordDefault)
            //{
            //    var isUserPasswordDefault = new Claim(ClaimTypeHelpers.IsDefaultPassword, user.IsPasswordDefault ? "Yes" : "No");
            //    isUserPasswordDefault.SetDestinations(Destinations.AccessToken);
            //    identity.AddClaim(isUserPasswordDefault);
            //}

            var roles = await _userManager.GetRolesAsync(user);
            var userRole = roles.FirstOrDefault();

            identity.AddClaim(new Claim(Scopes.Roles, userRole));
            return principal;
        }

        private void AddUserClaims(AppUsers user, ClaimsIdentity identity)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (identity == null)
                throw new ArgumentNullException(nameof(identity));

            if (!string.IsNullOrEmpty(user.FirstName))
                identity.AddClaim(new Claim(ClaimTypeHelpers.FirstName, user.FirstName));

            if (!string.IsNullOrEmpty(user.LastName))
                identity.AddClaim(new Claim(ClaimTypeHelpers.LastName, user.LastName));

            if (!string.IsNullOrEmpty(user.Email))
                identity.AddClaim(new Claim(OpenIddictConstants.Claims.Email, user.Email));

            if (user.LastLoginDate.HasValue)
                identity.AddClaim(new Claim(ClaimTypeHelpers.LastLogin, user.LastLoginDate.Value.ToDateString("dd/MM/yyyy")));
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
    }
}
