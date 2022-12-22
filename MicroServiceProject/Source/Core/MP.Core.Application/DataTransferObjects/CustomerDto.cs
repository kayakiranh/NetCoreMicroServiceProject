using MP.Core.Domain.Entities;
using System;

namespace MP.Core.Application.DataTransferObjects
{
    /// <summary>
    /// Customer data transfer object
    /// </summary>
    [Serializable]
    public class CustomerDto : BaseEntity
    {
        public string FullName { get; set; } = string.Empty; //Full name, Hüseyin Kayakıran
        public string IdentityNumber { get; set; } = string.Empty; //Identity number, 1234567890
        public string Phone { get; set; } = string.Empty; //Phone number with country code, +905001112233
        public string EmailAddress { get; set; } = string.Empty; //Email address, kayakiranh@gmail.com
        public decimal MonthlyIncome { get; set; } = 0; //Monthly income, salary + other incomes
        public string Token { get; set; } = ""; //JWT token
    }
}