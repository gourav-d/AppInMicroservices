using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
 {
     config.AddJsonFile($"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true);
 });

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConfiguration(builder.Configuration.GetSection(" Logging"));
    logging.AddConsole();
    logging.AddDebug();
});

builder.Services.AddOcelot().AddCacheManager(settings => settings.WithDictionaryHandle());
var app = builder.Build();

app.UseRouting();
//app.MapGet("/", () => "Hello World!");

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Hello World!");
    });
});

app.UseOcelot().Wait();

app.Run();
