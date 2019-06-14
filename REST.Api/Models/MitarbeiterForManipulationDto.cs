using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Models
{
    public abstract class MitarbeiterForManipulationDto
    {
        [Required]
        [MaxLength(100, ErrorMessage ="Bitte nicht mehr wie 100 Zeichen eingeben")]
        public virtual string Vorname { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Bitte nicht mehr wie 100 Zeichen eingeben")]
        public virtual string Nachname { get; set; }

        [Required(ErrorMessage = "Ein Geburtsdatum ist erforderlich.")]
        public DateTimeOffset GebDatum { get; set; }
    }
}
