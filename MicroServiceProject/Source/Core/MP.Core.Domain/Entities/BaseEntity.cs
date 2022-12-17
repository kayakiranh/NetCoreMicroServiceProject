using MP.Core.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace MP.Core.Domain.Entities
{
    [Serializable]
    public class BaseEntity
    {
        [Key]
        public long Id { get; set; } = 0; //Int primary key, 0,1,2,3,..
        public DateTime Created { get; set; } = DateTime.Now; //Created Time
        [Range(0, 2)]
        public int Status { get; set; } = (int)EntityStatus.None; //Status
    }
}