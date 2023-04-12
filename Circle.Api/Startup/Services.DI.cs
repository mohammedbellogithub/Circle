using Circle.Core.Services.User;
using Circle.Shared.Configs;
using Circle.Shared.Context;
using Circle.Shared.Dapper.Interfaces;
using Circle.Shared.Dapper.Repository;
using Circle.Shared.Models.OpenIddict;
using Circle.Shared.Models.UserIdentity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static OpenIddict.Abstractions.OpenIddictConstants;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Reflection;
using Circle.Core.Validations;
using FluentValidation;
using FluentValidation.AspNetCore;
using Circle.Core.ViewModels.User;

namespace Circle.Api.Startup
{
    public static class ServicesConfiguration
    {
        
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddControllers();
            services.AddDbContext<CircleDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default")));

            services.AddSingleton<IDbConnection>(db =>
            {
                var connectionString = Configuration.GetConnectionString("Default");
                var connection = new SqlConnection(connectionString);
                return connection;
            });

            // Add services to the container.
            services.AddTransient<IUserService, UserService>();

            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

            services.AddCors(options =>
            {
                options.AddPolicy("circle-cors", builder =>
                {
                    //for when you're running on localhost
                    builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
                    .AllowAnyHeader().AllowAnyMethod();


                    //builder.WithOrigins("http://localhost:3000/");
                });
            });
            //services.AddCors(o => o.AddPolicy("walure-cors", builder =>
            //{
            //    var settings = Configuration.GetSection("AppSettings").Get<AppSettings>();

            //    if (HostingEnvironment.IsDevelopment())
            //    {
            //        builder.WithOrigins(settings.CORS_ORIGIN)
            //          .AllowAnyMethod()
            //          .AllowAnyHeader()
            //          .AllowCredentials();
            //    }
            //    else
            //    {
            //        builder.WithOrigins(settings.CORS_ORIGIN)
            //           .AllowAnyMethod()
            //           .AllowAnyHeader()
            //           .AllowCredentials();
            //    }
            //}));

            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
               .RequireAuthenticatedUser().Build();

                options.Filters.Add(new AuthorizeFilter(policy));
            });
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<UserRegisterationViewModelValidator>();


            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c => {
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.IgnoreObsoleteActions();
                c.IgnoreObsoleteProperties();
                c.CustomSchemaIds(type => type.FullName);
            });
            return services;

        }

    }
}
