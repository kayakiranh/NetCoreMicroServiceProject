using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MP.Core.Domain.Entities
{
    /// <summary>
    /// Credit card class based base entity.
    /// Banks insert credit cards
    /// Banks determine to limits and minimum financial score. Like an American Express credit cards needed higher financial scores.
    /// </summary>
    [Serializable]
    public class CreditCard : BaseEntity
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; } //Card name, Bonus Card

        [Required]
        [Range(0, 2)]
        public string TypeName { get; set; } //Card name, Bonus Card

        [Required]
        [Range(0, 2)]
        public string BankName { get; set; } //Bank name, Garanti

        [Required]
        [Range(1, 5)]
        public double MinimumFinancialScore { get; set; } //Minimum financial score, 1 to 5

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MinimumLimit { get; set; } //Minimum limit, 500

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MaximumLimit { get; set; } //Maximum limit, 10000

    }
}