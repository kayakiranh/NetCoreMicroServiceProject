using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP.Core.Domain.Entities
{
    [Serializable]
    public class Application : BaseEntity
    {
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public int CreditCardId { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public string FilePath { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual CreditCard CreditCard { get; set; }

    }
}