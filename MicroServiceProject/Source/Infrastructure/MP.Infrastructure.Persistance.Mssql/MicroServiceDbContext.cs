using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MP.Core.Domain.Entities;

namespace MP.Infrastructure.Persistance.Mssql
{
    /// <summary>
    /// Context Class
    /// Used lazy loading
    /// </summary>
    public partial class MicroServiceDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public MicroServiceDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetSection("ConnectionStrings:MsSqlConnectionString").Value);
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.EnableDetailedErrors();
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}