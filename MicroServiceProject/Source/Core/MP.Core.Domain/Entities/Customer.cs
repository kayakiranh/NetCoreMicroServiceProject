using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MP.Core.Domain.Entities
{
    /// <summary>
    /// Customer class based base entity.
    /// Customer login app then they could apply what credit card want.
    /// </summary>
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
        [Column(TypeName = "decimal(18,2)")]
        public decimal MonthlyIncome { get; set; } = 0; //Monthly income, salary + other incomes

        [Required]
        public string Token { get; set; } = string.Empty; //JWT token

        //Empty Customer
        public static Customer EmptyCustomer()
        {
            return new Customer
            {
                Id = 0,
                Created = DateTime.Now,
                Status = 0,
                FullName = string.Empty,
                IdentityNumber = string.Empty,
                Phone = string.Empty,
                EmailAddress = string.Empty,
                Password = string.Empty,
                MonthlyIncome = 0,
                Token = string.Empty
            };
        }
    }
}