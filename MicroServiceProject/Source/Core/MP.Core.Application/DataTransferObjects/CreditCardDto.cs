using MP.Core.Domain.Entities;
using System;

namespace MP.Core.Application.DataTransferObjects
{
    [Serializable]
    public class CreditCardDto : BaseEntity
    {
        public string Name { get; set; } //Card name, Bonus Card
        public string TypeName { get; set; } //Card name, Bonus Card
        public string BankName { get; set; } //Bank name, Garanti
        public double MinimumFinancialScore { get; set; } //Minimum financial score, 1 to 5
        public decimal MinimumLimit { get; set; } //Minimum limit, 1000
        public decimal MaximumLimit { get; set; } //Maximum limit, 10000
    }
}