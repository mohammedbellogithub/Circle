using Circle.Shared.Configs;
using Circle.Shared.Context;
using Circle.Shared.Models.OpenIddict;
using Circle.Shared.Models.UserIdentity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using static OpenIddict.Abstractions.OpenIddictConstants;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Hosting.Internal;

namespace Circle.Api
{
    public static partial class Startup
    {
        public static WebApplicationBuilder ConfigureAuthServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<AppUsers, AppRoles>(options =>
            {
                //to be reconfigured
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
                options.Lockout.AllowedForNewUsers = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Lockout.MaxFailedAccessAttempts = 8;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedAccount = true;

            }).AddEntityFrameworkStores<CircleDbContext>()
         .AddDefaultTokenProviders()
         .AddSignInManager<SignInManager<AppUsers>>();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = Claims.Role;
            });

            var authSettings = new AuthSettings();
            builder.Configuration.Bind(nameof(AuthSettings), authSettings);
            var tokenExpiry = TimeSpan.FromMinutes(authSettings.TokenExpiry);

            builder.Services.AddOpenIddict()
            .AddCore(options =>
            {
                // Configure OpenIddict to use the Entity Framework Core stores and models.
                // Note: call ReplaceDefaultEntities() to replace the default entities.
                options.UseEntityFrameworkCore()
                    .UseDbContext<CircleDbContext>()
                    .ReplaceDefaultEntities<CircleOpenIddictApplication, CircleOpenIddictAuthorization, CircleOpenIddictScope, CircleOpenIddictToken, Guid>();
            })
            .AddServer(options =>
            {
                options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Address, Scopes.Phone,
                    Scopes.Roles, Scopes.OfflineAccess, Scopes.OpenId);

                options.DisableAccessTokenEncryption()
                .SetTokenEndpointUris("/api/auth/token")
                .SetUserinfoEndpointUris("/api/auth/userinfo")
                .SetRevocationEndpointUris("/api/auth/revoke")
                .AllowRefreshTokenFlow()
                .AcceptAnonymousClients() //to be studied
                .AllowPasswordFlow()
                .SetAccessTokenLifetime(tokenExpiry)
                .SetIdentityTokenLifetime(tokenExpiry)
                .SetRefreshTokenLifetime(tokenExpiry);


                if (!authSettings.RequireHttps)
                {
                    //Register the signing and encryption credentials.
                    options.AddDevelopmentEncryptionCertificate()
                           .AddDevelopmentSigningCertificate();
                }
                else
                {
                     byte[] rawData = File.ReadAllBytes(Path.Combine(builder.Environment.ContentRootPath,
                       "wwwroot", "dev_cert.pfx"));

                    var x509Certificate = new X509Certificate2(rawData, authSettings.Password, X509KeyStorageFlags.MachineKeySet |
                        X509KeyStorageFlags.Exportable);

                    options.AddEncryptionCertificate(x509Certificate).AddSigningCertificate(x509Certificate);
                }



                // Register the ASP.NET Core host and configure the ASP.NET Core options.
                options.UseAspNetCore()
                     .EnableAuthorizationEndpointPassthrough()
                     .EnableLogoutEndpointPassthrough()
                     .EnableStatusCodePagesIntegration()
                     .EnableTokenEndpointPassthrough();


            })
            .AddValidation(options =>
            {
                // Import the configuration from the local OpenIddict server instance.
                options.UseLocalServer();
                // Register the ASP.NET Core host.
                options.UseAspNetCore();
            });

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;

                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = authSettings.Issuer,
                    ValidAudience = authSettings.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.SecretKey)),
                };
            }); ;

            builder.Services.AddAuthorization();

            return builder;
        }
    }
}
