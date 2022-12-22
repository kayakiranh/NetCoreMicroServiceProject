using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MP.Core.Application.Repositories;
using MP.Core.Domain.Entities;
using MP.Core.Domain.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MP.Infrastructure.Persistance.Mssql.Repositories
{
    /// <summary>
    /// Credit card repository
    /// </summary>
    public class CreditCardRepository : GenericRepository<CreditCard>, ICreditCardRepository
    {
        private readonly MicroServiceDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public CreditCardRepository(MicroServiceDbContext dbContext, IConfiguration configuration) : base(dbContext, configuration)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public async Task<List<CreditCard>> ListByBank(BankNames bankName)
        {
            return await _dbContext.CreditCards.Where(x => x.BankName == bankName.ToString()).ToListAsync();
        }

        public async Task<List<CreditCard>> ListByFinancialScore(double minScore = 0, double maxScore = 5)
        {
            return await _dbContext.CreditCards.Where(x => x.MinimumFinancialScore >= minScore && x.MinimumFinancialScore <= maxScore).ToListAsync();
        }

        public async Task<List<CreditCard>> ListByType(CreditCardTypes creditCardType)
        {
            return await _dbContext.CreditCards.Where(x => x.TypeName == creditCardType.ToString()).ToListAsync();
        }
    }
}