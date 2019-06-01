using System;
using System.ComponentModel.DataAnnotations;

namespace REST.Api.Entities
{
    public class Schlag
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        //[Required]
        //public int Groesse { get; set; }
    }
}
