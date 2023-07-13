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

namespace Circle.Api
{
    public static partial class Startup
    {
        public static WebApplicationBuilder RegisterDI(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();

            builder.Services.AddDbContext<CircleDbContext>(options =>
            {
                var connectionstring = builder.Configuration.GetConnectionString("Default");

                options.UseSqlServer(connectionstring);
            });

           
            // Add services to the container.
            builder.Services.AddTransient<IUserService, UserService>();

            return builder;

        }
    }
}
