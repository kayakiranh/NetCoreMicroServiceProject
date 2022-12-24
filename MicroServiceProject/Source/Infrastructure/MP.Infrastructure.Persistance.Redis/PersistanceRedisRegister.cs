using Microsoft.Extensions.DependencyInjection;
using MP.Core.Application.Repositories;

namespace MP.Infrastructure.Persistance.Redis
{
    /// <summary>
    /// Dependency injection settings
    /// </summary>
    public static class PersistanceRedisRegister
    {
        public static void Register(this IServiceCollection services)
        {
            services.AddTransient<ICacheRepository, CacheRepository>();
        }
    }
}