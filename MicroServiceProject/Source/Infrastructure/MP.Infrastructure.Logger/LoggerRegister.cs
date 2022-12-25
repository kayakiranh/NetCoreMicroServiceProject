using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
            services.AddSingleton<ILogger>(provider =>  provider.GetRequiredService<ILogger>());
        }
    }
}