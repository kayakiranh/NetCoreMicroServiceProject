using System;
using System.ComponentModel.DataAnnotations;

namespace MP.Core.Domain.Entities
{
    [Serializable]
    public class Customer : BaseEntity
    {
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string FullName { get; set; } = string.Empty; //Full name, Hüseyin Kayakıran

        [Required]
        [StringLength(15, MinimumLength = 7)]
        public string IdentityNumber { get; set; } = string.Empty; //Identity number, 1234567890

        [Required]
        [StringLength(15, MinimumLength = 7)]
        public string Phone { get; set; } = string.Empty; //Phone number with country code, +905001112233

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = string.Empty; //Email address, kayakiranh@gmail.com

        [Required]
        [MinLength(10)]
        public string Password { get; set; } = string.Empty; //Password, sha + salt

        [Required]
        [Range(0, int.MaxValue)]
        public decimal MonthlyIncome { get; set; } = 0; //Monthly income, salary + other incomes
    }
}