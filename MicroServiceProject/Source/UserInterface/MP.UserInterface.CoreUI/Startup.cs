using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MP.Infrastructure.Persistance.Mssql;

namespace MP.UserInterface.CoreUI
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            PersistanceMssqlRegister.Register(services);
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