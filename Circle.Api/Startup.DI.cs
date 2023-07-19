using Circle.Core.Services.User;
using Circle.Shared.Context;
using Circle.Shared.Dapper.Interfaces;
using Circle.Shared.Dapper.Repository;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Circle.Core.Services.Email;
using Circle.Core.Services.Cache;
using Circle.Core.Components.Policy;
using Circle.Core.Registration;
using Circle.Core.Services.Businesses;
using StackExchange.Redis;

namespace Circle.Api
{
    public static partial class Startup
    {
        public static WebApplicationBuilder RegisterDI(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();

            var redisMultiplexer = ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection"));
            builder.Services.AddSingleton<IConnectionMultiplexer>(redisMultiplexer);
            builder.Services.AddSingleton<ICacheService, CacheService>();


            builder.Services.AddDbContext<CircleDbContext>(options =>
            {
                var connectionstring = builder.Configuration.GetConnectionString("Default");

                options.UseSqlServer(connectionstring);
            });


            // Add services to the container
            RepositoryRegistration.RepositoryRegDI(builder);
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IEmailService, EmailService>();
            builder.Services.AddTransient<IAuthorizationHandler, PermissionsAuthorizationHandler>();
            builder.Services.AddTransient<IBusinessService, BusinessService>();


            return builder;

        }
    }
}
