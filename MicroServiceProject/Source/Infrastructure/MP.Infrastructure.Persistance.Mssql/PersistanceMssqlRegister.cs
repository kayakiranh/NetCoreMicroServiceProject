using Microsoft.Extensions.DependencyInjection;
using MP.Core.Application.Repositories;
using MP.Infrastructure.Persistance.Mssql.Repositories;

namespace MP.Infrastructure.Persistance.Mssql
{
    public static class PersistanceMssqlRegister
    {
        /// <summary>
        /// DI
        /// </summary>
        /// <param name="services"></param>
        public static void Register(this IServiceCollection services)
        {
            services.AddDbContext<MicroServiceDbContext>(ServiceLifetime.Scoped);
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<ICreditCardRepository, CreditCardRepository>();
        }
    }
}