using Microsoft.Extensions.DependencyInjection;
using MP.Core.Application.Repositories;

namespace MP.Infrastructure.Logger
{
    /// <summary>
    /// Dependency injection settings
    /// </summary>
    public static class LoggerRegister
    {
        public static void Register(this IServiceCollection services)
        {
            services.AddTransient<ILoggerRepository, LoggerRepository>();
        }
    }
}