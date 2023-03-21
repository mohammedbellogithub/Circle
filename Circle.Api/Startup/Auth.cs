using Circle.Shared.Configs;
using Circle.Shared.Context;
using Circle.Shared.Models.OpenIddict;
using Circle.Shared.Models.UserIdentity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static OpenIddict.Abstractions.OpenIddictConstants;
namespace Circle.Api.Startup
{
    public static class Auth
    {
        public static IServiceCollection ConfigureAuthServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddIdentity<AppUsers, AppRoles>(options =>
            {
                //to be reconfigured
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
                options.Lockout.AllowedForNewUsers = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;

            }).AddEntityFrameworkStores<CircleDbContext>()
         .AddDefaultTokenProviders()
         .AddSignInManager<SignInManager<AppUsers>>();

            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = Claims.Role;
            });

            var authSettings = new AuthSettings();
            Configuration.Bind(nameof(AuthSettings), authSettings);
            var tokenExpiry = TimeSpan.FromMinutes(authSettings.TokenExpiry);

            services.AddOpenIddict()
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


                options.SetTokenEndpointUris("/api/auth/token")
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
                    options.UseAspNetCore(configure =>
                    {
                        configure.DisableTransportSecurityRequirement();
                    });

                    //Register the signing and encryption credentials.
                    options.AddDevelopmentEncryptionCertificate()
                           .AddDevelopmentSigningCertificate();
                }
                else
                {
                    //byte[] rawData = File.ReadAllBytes(Path.Combine(HostingEnvironment.ContentRootPath, "App_Data", "walure-bo.pfx"));
                    //var x509Certificate = new X509Certificate2(rawData,
                    //    authSettings.Password,
                    //    X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);

                    //options.AddEncryptionCertificate(x509Certificate).AddSigningCertificate(x509Certificate);
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

            services.AddAuthentication(x =>
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


            return services;
        }
    }
}
