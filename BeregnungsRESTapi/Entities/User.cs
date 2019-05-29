using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeregnungsRESTapi.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Mail { get; set; }

        [Required]
        [MaxLength(50)]
        public string Password { get; set; }

        [Required]
        [MaxLength(50)]
        public string Firma { get; set; }

    }
}
