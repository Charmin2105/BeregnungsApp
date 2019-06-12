using System;
using System.ComponentModel.DataAnnotations;

namespace REST.Api.Entities
{
    public class Account
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Benutzername { get; set; }

        [Required]
        [MaxLength(100)]
        public string Passwort { get; set; }

        [Required]
        [MaxLength(100)]
        public bool istAdmin { get; set; }


    }
}