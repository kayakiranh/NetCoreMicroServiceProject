using Microsoft.Extensions.DependencyInjection;
using MP.Core.Application.Repositories;
using MP.Infrastructure.Persistance.Mssql.Repositories;

namespace MP.Infrastructure.Persistance.Mssql
{
    /// <summary>
    /// Dependency injection settings
    /// </summary>
    public static class PersistanceMssqlRegister
    {
        public static void Register(this IServiceCollection services)
        {
            services.AddDbContext<MicroServiceDbContext>(ServiceLifetime.Scoped);
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<ICreditCardRepository, CreditCardRepository>();
        }
    }
}