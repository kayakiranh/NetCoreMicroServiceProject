using MP.Core.Domain.Entities;
using System.Threading.Tasks;

namespace MP.Core.Application.Repositories
{
    /// <summary>
    /// Customer repository
    /// </summary>
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        public Task<Customer> Login(string email, string password);
        public Task<Customer> GetByIdentityNumber(string identityNumber);
        public Task<Customer> GetByToken(string token);
        public void UpdateToken(long id, string token);
    }
}