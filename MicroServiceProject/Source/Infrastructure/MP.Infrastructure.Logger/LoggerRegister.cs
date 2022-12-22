using Microsoft.Extensions.DependencyInjection;
using MP.Core.Application.Repositories;

namespace MP.Infrastructure.Logger
{
    public static class LoggerRegister
    {
        /// <summary>
        /// DI
        /// </summary>
        /// <param name="services"></param>
        public static void Register(this IServiceCollection services)
        {
            services.AddTransient<ILoggerRepository, LoggerRepository>();
        }
    }
}