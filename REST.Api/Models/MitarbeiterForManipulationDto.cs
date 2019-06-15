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
        /// virtual string Vorname
        /// </summary>
        [Required]
        [MaxLength(100, ErrorMessage ="Bitte nicht mehr wie 100 Zeichen eingeben")]
        public virtual string Vorname { get; set; }

        /// <summary>
        /// virtual string Nachname 
        /// </summary>
        [Required]
        [MaxLength(100, ErrorMessage = "Bitte nicht mehr wie 100 Zeichen eingeben")]
        public virtual string Nachname { get; set; }

        /// <summary>
        ///  DateTimeOffset GebDatum
        /// </summary>
        [Required(ErrorMessage = "Ein Geburtsdatum ist erforderlich.")]
        public DateTimeOffset GebDatum { get; set; }
    }
}
