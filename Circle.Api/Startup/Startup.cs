using Circle.Shared.Context;
using DbUp;
using DbUp.Engine.Output;
using DbUp.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System.Data;

namespace Circle.Api.Startup
{
    public  class Startup
    {
        public static WebApplication AppConfiguration(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders(); // has effect
            builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
            {
                var logConfig = loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration)
                        .Enrich.FromLogContext()
                        .WriteTo.File(@"logs\log.txt", rollingInterval: RollingInterval.Day,
                        restrictedToMinimumLevel: LogEventLevel.Information,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                        shared: true);

                if (!hostingContext.HostingEnvironment.IsDevelopment())
                {
                    //logConfig.WriteTo.Sentry(o =>
                    //{
                    //    o.Environment = hostingContext.HostingEnvironment.EnvironmentName;
                    //    // Debug and higher are stored as breadcrumbs (default is Information)
                    //    o.MinimumBreadcrumbLevel = LogEventLevel.Information;
                    //    // Warning and higher is sent as event (default is Error)
                    //    o.MinimumEventLevel = LogEventLevel.Error;
                    //    o.Dsn = hostingContext.Configuration.GetValue<string>("AppSettings:SentryUrl");
                    //});
                }
            });


            builder.Services.AddDbContext<CircleDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

            builder.Services.AddSingleton<IDbConnection>(db =>
            {
                var connectionString = builder.Configuration.GetConnectionString("Default");
                var connection = new SqlConnection(connectionString);
                return connection;
            });

            builder.Services.ConfigureServices(builder.Configuration);
            builder.Services.ConfigureAuthServices(builder.Configuration);


            //
            var app = builder.Build();
            SeedDatabase(app.Services).Wait();
            Configure(app);
            return app;

        }

        private static void Configure(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

        }

        public static async Task SeedDatabase(IServiceProvider _serviceProvider)
        {
            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<CircleDbContext>();
             await context.Database.EnsureCreatedAsync();

            TableMigrationScript(scope);
            StoredProcedureMigrationScript(scope);

            //var manager = scope.ServiceProvider.GetRequiredService<OpenIddictApplicationManager<WalureOpenIddictApplication>>();
            ////await OpenIddictManager.CreateClientApps(builder.Configuration["AuthSettings:Authority"], manager, cancellationToken);

            //if (await manager.FindByClientIdAsync(ClientConstant.BackOfficeClientId) is null)
            //{
            //    await manager.CreateAsync(new OpenIddictApplicationDescriptor
            //    {
            //        ClientId = ClientConstant.BackOfficeClientId,
            //        ClientSecret = ClientConstant.BackOfficeClientSecret,
            //        DisplayName = "Walure Back Office",
            //        Permissions =
            //        {
            //            Permissions.Endpoints.Token,
            //               Permissions.GrantTypes.Password,
            //               Permissions.GrantTypes.RefreshToken,
            //               Permissions.Scopes.Email,
            //               Permissions.Scopes.Profile,
            //               Permissions.Scopes.Roles,
            //               Permissions.Endpoints.Revocation
            //        }
            //    });
            //}
        }
        /// <summary>
        /// Sql migration for table Schema
        /// </summary>
        public static void TableMigrationScript(IServiceScope scope)
        {
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            string dbConnStr = configuration.GetConnectionString("Default");
            EnsureDatabase.For.SqlDatabase(dbConnStr);

            var _logger = scope.ServiceProvider.GetRequiredService<ILogger<Startup>>();

            var upgrader = DeployChanges.To.SqlDatabase(dbConnStr)
            .WithScriptsFromFileSystem(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sql", "Tables"))
            .WithTransactionPerScript()
            .JournalToSqlTable("dbo", "TableMigration")
            .LogTo(new SerilogDbUpLog(_logger))
            .LogToConsole()
            .Build();
            upgrader.PerformUpgrade();
        }

        /// <summary>
        /// Sql migration for stored procedure
        /// </summary>
        public static void StoredProcedureMigrationScript(IServiceScope scope)
        {
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            string dbConnStr = configuration.GetConnectionString("Default");
            EnsureDatabase.For.SqlDatabase(dbConnStr);

            var _logger = scope.ServiceProvider.GetRequiredService<ILogger<Startup>>();
            var upgrader = DeployChanges.To.SqlDatabase(dbConnStr)
            .WithScriptsFromFileSystem(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sql", "Sprocs"))
            .WithTransactionPerScript()
            .JournalTo(new NullJournal())
            .LogTo(new SerilogDbUpLog(_logger))
            .LogToConsole()
            .Build();

            upgrader.PerformUpgrade();
        }
    }

     public class SerilogDbUpLog : IUpgradeLog
    {
        private readonly ILogger<Startup> _logger;

        public SerilogDbUpLog(ILogger<Startup> logger)
        {
            _logger = logger;
        }

        public void WriteError(string format, params object[] args)
        {
            _logger.LogError(format, args);
        }

        public void WriteInformation(string format, params object[] args)
        {
            _logger.LogInformation(format, args);
        }

        public void WriteWarning(string format, params object[] args)
        {
            _logger.LogWarning(format, args);
        }
    }


}

