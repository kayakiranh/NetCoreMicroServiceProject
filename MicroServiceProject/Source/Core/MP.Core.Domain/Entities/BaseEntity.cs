using MP.Core.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace MP.Core.Domain.Entities
{
    /// <summary>
    /// Base entity, carry common properties to related classes.
    /// </summary>
    [Serializable]
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; } //Int primary key, 0,1,2,3,..
        public DateTime Created { get; set; } = DateTime.Now; //Created Time
        public int Status { get; set; } = (int)EntityStatus.None; //Status
    }
}