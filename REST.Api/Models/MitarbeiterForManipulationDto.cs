using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Models
{
    /// <summary>
    /// abstract class MitarbeiterForManipulationDto
    /// </summary>
    public abstract class MitarbeiterForManipulationDto
    {
        /// <summary>
        /// Vorname des Mitarbeiter
        /// </summary>
        [Required]
        [MaxLength(100, ErrorMessage ="Bitte nicht mehr wie 100 Zeichen eingeben")]
        public virtual string Vorname { get; set; }

        /// <summary>
        /// Nachname  des Mitarbeiter
        /// </summary>
        [Required]
        [MaxLength(100, ErrorMessage = "Bitte nicht mehr wie 100 Zeichen eingeben")]
        public virtual string Nachname { get; set; }

        /// <summary>
        /// GebDatum  des Mitarbeiter
        /// </summary>
        [Required(ErrorMessage = "Ein Geburtsdatum ist erforderlich.")]
        public DateTimeOffset Geburtstag { get; set; }
    }
}
