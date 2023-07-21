using Circle.Api;

var builder = WebApplication.CreateBuilder(args)
    .RegisterServices()
    .RegisterDI()
    .ConfigureSerilog();
    builder.Configuration
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
        .AddEnvironmentVariables();
    builder.ConfigureAuthServices();

var app = builder.Build();

app.ConfigureMiddleware();
app.Run();


