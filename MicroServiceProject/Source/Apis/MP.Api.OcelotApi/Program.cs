using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace MP.Api.OcelotApi
{
    public class Program
    {
            public static void Main(string[] args) { CreateHostBuilder(args).Build().Run(); }

            public static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args).ConfigureServices(services => { 
                    services.AddOcelot();
                }).ConfigureAppConfiguration((host, config) => {
                    config.AddJsonFile("ocelot.json");
                }).ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.ConfigureKestrel(serverOptions => serverOptions.AddServerHeader = false).UseStartup<Startup>().Configure(async app => await app.UseOcelot()).UseUrls("http://localhost:5000");
                });
    }
}