using Microsoft.Extensions.DependencyInjection;
using MP.Core.Application.Repositories;
using MP.Infrastructure.Logger;
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
            services.AddDbContext<MicroServiceDbContext>(ServiceLifetime.Singleton);
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICreditCardRepository, CreditCardRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            LoggerRegister.Register(services);
        }
    }
}