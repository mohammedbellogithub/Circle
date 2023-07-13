using Circle.Api;

var builder = WebApplication.CreateBuilder(args)
    .RegisterServices()
    .RegisterDI()
    .ConfigureSerilog()
    .ConfigureAuthServices();

var app = builder.Build();

app.ConfigureMiddleware();
app.Run();


