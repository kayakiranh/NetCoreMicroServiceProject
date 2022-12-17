using MP.Core.Domain.Entities;
using MP.Core.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MP.Core.Application.Repositories
{
    /// <summary>
    /// Credit card repository
    /// </summary>
    public interface ICreditCardRepository : IGenericRepository<CreditCard>
    {
        public Task<List<CreditCard>> ListByFinancialScore(double minScore = 0, double maxScore = 5);
        public Task<List<CreditCard>> ListByType(CreditCardTypes creditCardType);
        public Task<List<CreditCard>> ListByBank(BankNames bankName);
    }
}