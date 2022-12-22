using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MP.Core.Application.Repositories;
using MP.Core.Domain.Entities;
using MP.Core.Domain.Enums;
using MP.Infrastructure.Helper;
using System.Threading.Tasks;

namespace MP.Infrastructure.Persistance.Mssql.Repositories
{
    /// <summary>
    /// Customer repository
    /// </summary>
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly MicroServiceDbContext _dbContext;

        public CustomerRepository(MicroServiceDbContext dbContext, IConfiguration configuration) : base(dbContext, configuration)
        {
            _dbContext = dbContext;
        }

        public async Task<Customer> GetByIdentityNumber(string identityNumber)
        {
            if (identityNumber.StringSecurityValidation() == "") return new Customer();

            return await _dbContext.Customers.SingleOrDefaultAsync(x => x.IdentityNumber == identityNumber);
        }

        public async Task<Customer> GetByToken(string token)
        {
            if (token.StringSecurityValidation() == "") return new Customer();

            return await _dbContext.Customers.SingleOrDefaultAsync(x => x.Token == token);
        }

        public async Task<Customer> Login(string email, string password)
        {
            if (email.MailValidation() == "" || password.StringSecurityValidation() == "") return new Customer();
            password = password.Encrypt();

            return await _dbContext.Customers.SingleOrDefaultAsync(x => x.EmailAddress == email && x.Password == password && x.Status == (int)EntityStatus.Active);
        }

        public async void UpdateToken(long id, string token)
        {
            if(id > 0 && token.StringSecurityValidation() != "")
            {
                Customer customer = await GetById(id);
                if (customer.Id > 0)
                {
                    customer.Token = token;
                    _dbContext.Customers.Update(customer);
                    _dbContext.SaveChanges();
                }
            }
        }
    }
}