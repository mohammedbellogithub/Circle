using Circle.Shared.Constants;
using Circle.Shared.Context;
using Circle.Shared.Models.OpenIddict;
using DbUp.Helpers;
using DbUp;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using static OpenIddict.Abstractions.OpenIddictConstants;
using DbUp.Engine.Output;
using Circle.Shared.Helpers;

namespace Circle.Api
{
    public static partial class Startup
    {
        public static WebApplication ConfigureMiddleware(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("circle-cors");
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            TableMigrationScript(app);
            StoredProcedureMigrationScript(app);
            SeedDatabase(app).Wait();

            WebHelpers.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());

            return app;
        }

        

        public static async Task SeedDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<CircleDbContext>();
            await context.Database.EnsureCreatedAsync();


            var manager = scope.ServiceProvider.GetRequiredService<OpenIddictApplicationManager<CircleOpenIddictApplication>>();

            if (await manager.FindByClientIdAsync(ClientConstant.BackOfficeClientId) is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = ClientConstant.BackOfficeClientId,
                    ClientSecret = ClientConstant.BackOfficeClientSecret,
                    DisplayName = "Circle System",
                    Permissions =
                    {
                        Permissions.Endpoints.Token,
                           Permissions.GrantTypes.Password,
                           Permissions.GrantTypes.RefreshToken,
                           Permissions.Scopes.Email,
                           Permissions.Scopes.Profile,
                           Permissions.Scopes.Roles,
                           Permissions.Endpoints.Revocation
                    }
                });
            }
        }
        /// <summary>
        /// Sql migration for table Schema
        /// </summary>
        public static void TableMigrationScript(this WebApplication app)
        {
            string dbConnStr = app.Configuration.GetConnectionString("Default");
            EnsureDatabase.For.SqlDatabase(dbConnStr);
            var upgrader = DeployChanges.To.SqlDatabase(dbConnStr)
            .WithScriptsFromFileSystem(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sql", "Tables"))
            .WithTransactionPerScript()
            .JournalToSqlTable("dbo", "TableMigration")
            .LogTo(new SerilogDbUpLog(app.Logger))
            .LogToConsole()
            .Build();
            upgrader.PerformUpgrade();
        }

        /// <summary>
        /// Sql migration for stored procedure
        /// </summary>
        public static void StoredProcedureMigrationScript(this WebApplication app)
        {
            string dbConnStr = app.Configuration.GetConnectionString("Default");
            EnsureDatabase.For.SqlDatabase(dbConnStr);
            var upgrader = DeployChanges.To.SqlDatabase(dbConnStr)
            .WithScriptsFromFileSystem(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sql", "Sprocs"))
            .WithTransactionPerScript()
            .JournalTo(new NullJournal())
            .JournalToSqlTable("dbo", "SprocsMigration")
            .LogTo(new SerilogDbUpLog(app.Logger))
            .LogToConsole()
            .Build();

            upgrader.PerformUpgrade();
        }
    }

    public class SerilogDbUpLog : IUpgradeLog
    {
        private readonly ILogger _logger;

        public SerilogDbUpLog(Microsoft.Extensions.Logging.ILogger logger)
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
