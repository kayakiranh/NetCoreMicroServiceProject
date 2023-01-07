using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MP.Infrastructure.Logger;
using MP.Infrastructure.Mailer;
using MP.Infrastructure.Persistance.Mssql;
using System.IO;
using System.IO.Compression;
using System.Reflection;

namespace MP.Api.OcelotApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env) { Configuration = configuration; Env = env; }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("ocelot.json", false).AddJsonFile("appsettings.json", false).Build();
            services.AddOptions();
            services.AddSingleton(configuration);
            services.AddControllers();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            PersistanceMssqlRegister.Register(services);
            MailerRegister.Register(services);
            LoggerRegister.Register(services);
            services.AddLogging();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddCors(options => { options.AddDefaultPolicy(builder => { builder.WithOrigins("http://localhost").AllowAnyHeader().AllowAnyMethod(); }); });
            services.Configure<BrotliCompressionProviderOptions>(options => { options.Level = CompressionLevel.Optimal; });
            services.AddResponseCompression(options => { options.Providers.Add<BrotliCompressionProvider>(); });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) { app.UseDeveloperExceptionPage(); }
            app.Use(async (context, next) => { await next(); });
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