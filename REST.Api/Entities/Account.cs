using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Entities
{
    public class Account
    {
        /// <summary>
        /// Guid ID
        /// </summary>
        [Key]
        public Guid ID { get; set; }

        /// <summary>
        /// string Benutzernamen
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Benutzername { get; set; }

        /// <summary>
        /// string Benutzernamen
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string EMail { get; set; }

        /// <summary>
        /// string Passwort
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Passwort { get; set; }

        /// <summary>
        /// bool IstAdmin
        /// </summary>
        [Required]
        public bool IstAdmin { get; set; }
    }
}
