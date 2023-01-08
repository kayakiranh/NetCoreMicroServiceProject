using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MP.Api.IdentityApi
{
    public static class Program
    {
        public static void Main(string[] args) { CreateHostBuilder(args).Build().Run(); }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => { webBuilder.ConfigureKestrel(serverOptions => serverOptions.AddServerHeader = false).UseStartup<Startup>().UseUrls("http://localhost:44305"); });
    }
}