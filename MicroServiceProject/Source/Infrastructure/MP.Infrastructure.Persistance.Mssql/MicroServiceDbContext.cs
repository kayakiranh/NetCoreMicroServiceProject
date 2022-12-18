using Microsoft.EntityFrameworkCore;
using MP.Core.Domain.Entities;
using MP.Infrastructure.Helper;

namespace MP.Infrastructure.Persistance.Mssql
{
    /// <summary>
    /// Context Class
    /// Used lazy loading
    /// </summary>
    public partial class MicroServiceDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(AppSettingsReadHelper.ReadByValue("Configurations", "ConnectionString"));
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.EnableDetailedErrors();
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}