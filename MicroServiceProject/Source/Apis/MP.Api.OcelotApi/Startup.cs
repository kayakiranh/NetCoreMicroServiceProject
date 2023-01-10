using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MP.Infrastructure.Logger;
using MP.Infrastructure.Mailer;
using MP.Infrastructure.Persistance.Mssql;
using MP.Infrastructure.Persistance.Redis;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.IO.Compression;
using System.Reflection;

namespace MP.Api.OcelotApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env) { Configuration = configuration; Env = env; }

        public IWebHostEnvironment Env { get; }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOcelot();
            services.AddOptions();
            services.AddControllers();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            MailerRegister.Register(services);
            LoggerRegister.Register(services);
            PersistanceMssqlRegister.Register(services);
            PersistanceRedisRegister.Register(services);
            services.AddLogging();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddCors(options => { options.AddDefaultPolicy(builder => { builder.WithOrigins("http://localhost").AllowAnyHeader().AllowAnyMethod(); }); });
            services.Configure<BrotliCompressionProviderOptions>(options => { options.Level = CompressionLevel.Optimal; });
            services.AddResponseCompression(options => { options.Providers.Add<BrotliCompressionProvider>(); });
        }

        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use(async (context, next) => { await next(); });
            await app.UseOcelot();
            app.UseHttpsRedirection();
            app.UseResponseCompression();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors();
            app.UseHsts();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}