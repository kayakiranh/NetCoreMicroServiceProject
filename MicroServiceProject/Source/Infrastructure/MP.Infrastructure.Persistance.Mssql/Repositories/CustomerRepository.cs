using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MP.Core.Application.Repositories;
using MP.Core.Domain.Entities;
using MP.Core.Domain.Enums;
using MP.Infrastructure.Helper;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MP.Infrastructure.Persistance.Mssql.Repositories
{
    /// <summary>
    /// Customer repository
    /// </summary>
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly MicroServiceDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public CustomerRepository(MicroServiceDbContext dbContext, IConfiguration configuration) : base(dbContext, configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
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

            Customer customer = await _dbContext.Customers.SingleOrDefaultAsync(x => x.EmailAddress == email && x.Password == password && x.Status == (int)EntityStatus.Active);
            if (customer.Id == 0) return new Customer();

            JwtSecurityTokenHandler tokenHandler = new();
            byte[] tokenKey = Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Key").Value);
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.Name, email)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);

            customer.Token = tokenHandler.WriteToken(securityToken);
            _dbContext.Update(customer);

            return customer;
        }
    }
}