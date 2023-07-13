using Circle.Shared.Configs;
using Circle.Shared.Context;
using Circle.Shared.Dapper.Interfaces;
using Circle.Shared.Dapper.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using Serilog;
using Serilog.Events;
using System.Data;
using System.Reflection;

namespace Circle.Api
{
    public static partial class Startup
    {
        public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            
            builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

            builder.Services.AddSingleton<IDbConnection>(db =>
            {
                var connectionString = builder.Configuration.GetConnectionString("Default");
                var connection = new SqlConnection(connectionString);
                return connection;
            });
            builder.Services.AddCors(options => options.AddPolicy("circle-cors", policyBuilder =>
            {
                var settings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();
                policyBuilder.WithOrigins(settings.CORS_ORIGIN)
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
            }));

            builder.Services.AddControllers(o =>
            {
                var policy = new AuthorizationPolicyBuilder(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
                o.Filters.Add(new AuthorizeFilter(policy));
            });

            builder.Services.AddSwaggerGen(option =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                option.IncludeXmlComments(xmlPath);

                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Circle Api provider", Version = "v1" });

                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                    "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                    "Enter 'Bearer'  and then your token in the text input " +
                    "below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                option.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });
            return builder;
        }


        public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
            {
                var logConfig = loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration)
                   .Enrich.FromLogContext()
                   .WriteTo.File(@"logs\log.txt", rollingInterval: RollingInterval.Day,
                   restrictedToMinimumLevel: LogEventLevel.Information,
                   outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                   shared: true);

                if (!builder.Environment.IsDevelopment())
                {
                    logConfig.WriteTo.Sentry(o =>
                    {
                        o.Environment = hostingContext.HostingEnvironment.EnvironmentName;
                        // Debug and higher are stored as breadcrumbs (default is Information)
                        o.MinimumBreadcrumbLevel = LogEventLevel.Information;
                        // Warning and higher is sent as event (default is Error)
                        o.MinimumEventLevel = LogEventLevel.Error;
                        o.Dsn = hostingContext.Configuration.GetValue<string>("AppSettings:SentryUrl");
                    });
                }
            });
            return builder;
        }
    }
}
