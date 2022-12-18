using Microsoft.EntityFrameworkCore;
using MP.Core.Application.Repositories;
using MP.Core.Domain.Entities;
using MP.Core.Domain.Enums;
using System.Threading.Tasks;

namespace MP.Infrastructure.Persistance.Mssql.Repositories
{
    /// <summary>
    /// Customer repository
    /// </summary>
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly MicroServiceDbContext _dbContext;

        public CustomerRepository(MicroServiceDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            if (_dbContext.Database == null)
            {
                _dbContext = new MicroServiceDbContext();
            }
        }

        public async Task<Customer> GetByIdentityNumber(string identityNumber)
        {
            return await _dbContext.Customers.SingleOrDefaultAsync(x => x.IdentityNumber == identityNumber);
        }

        public async Task<Customer> Login(string email, string password)
        {
            return await _dbContext.Customers.SingleOrDefaultAsync(x => x.EmailAddress == email && x.Password == password && x.Status == (int)EntityStatus.Active);
        }
    }
}