using Microsoft.Extensions.DependencyInjection;
using MP.Core.Application.Repositories;

namespace MP.Infrastructure.Mailer
{
    /// <summary>
    /// Dependency injection settings
    /// </summary>
    public static class MailerRegister
    {
        public static void Register(this IServiceCollection services)
        {
            services.AddTransient<IMailerRepository, MailerRepository>();
        }
    }
}