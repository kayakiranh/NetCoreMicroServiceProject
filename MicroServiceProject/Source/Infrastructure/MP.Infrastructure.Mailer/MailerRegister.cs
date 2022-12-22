using Microsoft.Extensions.DependencyInjection;
using MP.Core.Application.Repositories;
namespace MP.Infrastructure.Mailer
{
    public static class MailerRegister
    {
        /// <summary>
        /// DI
        /// </summary>
        /// <param name="services"></param>
        public static void Register(this IServiceCollection services)
        {
            services.AddTransient<IMailerRepository, MailerRepository>();
        }
    }
}