using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.IO.Compression;

namespace MP.UserInterface.CoreUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        { Configuration = configuration; Env = env; }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", false).Build();
            services.AddOptions();
            services.AddSingleton(configuration);
            services.AddControllers();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.Configure<BrotliCompressionProviderOptions>(options => { options.Level = CompressionLevel.Optimal; });
            services.AddResponseCompression(options => { options.Providers.Add<BrotliCompressionProvider>(); });
            services.AddSession(options => { options.Cookie.SecurePolicy = CookieSecurePolicy.Always; });
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) { app.UseDeveloperExceptionPage(); }
            app.Use(async (context, next) => { await next(); });
            app.UseCookiePolicy(new CookiePolicyOptions { Secure = CookieSecurePolicy.Always });
            app.UseHttpsRedirection();
            app.UseResponseCompression();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors();
            app.UseHsts();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=CreditCard}/{action=List}");
            });
        }
    }
}