using Microsoft.Extensions.DependencyInjection;
using MP.Core.Application.Repositories;

namespace MP.Infrastructure.Persistance.Redis
{
    public static class PersistanceRedisRegister
    {
        /// <summary>
        /// DI
        /// </summary>
        /// <param name="services"></param>
        public static void Register(this IServiceCollection services)
        {
            services.AddTransient<ICacheRepository, CacheRepository>();
        }
    }
}