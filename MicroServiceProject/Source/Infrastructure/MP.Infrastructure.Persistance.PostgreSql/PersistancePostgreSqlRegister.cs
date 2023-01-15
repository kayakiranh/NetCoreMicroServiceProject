using Microsoft.Extensions.DependencyInjection;
using MP.Core.Application.Repositories;
using MP.Infrastructure.Logger;

namespace MP.Infrastructure.Persistance.PostgreSql
{
    /// <summary>
    /// Dependency injection settings
    /// </summary>
    public static class PersistancePostgreSqlRegister
    {
        public static void Register(this IServiceCollection services)
        {
            services.AddScoped<IPostgreSqlRepository, PostgreSqlRepository>();
            LoggerRegister.Register(services);
        }
    }
}