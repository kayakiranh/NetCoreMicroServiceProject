using System;

namespace MP.Core.Application.DataTransferObjects
{
    public class CreditCardApplicationDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int CustomerId { get; set; }
        public int CreditCardId { get; set; }
        public string FilePath { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}