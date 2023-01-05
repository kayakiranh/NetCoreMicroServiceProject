using Microsoft.Extensions.DependencyInjection;
using MP.Core.Application.Repositories;
using MP.Infrastructure.Logger;

namespace MP.Infrastructure.Mailer
{
    /// <summary>
    /// Dependency injection settings
    /// </summary>
    public static class MailerRegister
    {
        public static void Register(this IServiceCollection services)
        {
            services.AddScoped<IMailerRepository, MailerRepository>();
            LoggerRegister.Register(services);
        }
    }
}