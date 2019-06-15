using System;
using System.ComponentModel.DataAnnotations;

namespace REST.Api.Models
{
    /// <summary>
    ///  class BeregnungsDatenForManipulationDto
    /// </summary>
    public abstract class BeregnungsDatenForManipulationDto
    {
        /// <summary>
        /// DateTime StartDatum
        /// </summary>
        [Required(ErrorMessage = "Ein Startdatum ist erforderlich.")]
        public DateTime StartDatum { get; set; }

        /// <summary>
        /// DateTime StartUhrzeit 
        /// </summary>
        [Required(ErrorMessage = "Ein Uhrzeit ist erforderlich.")]
        public DateTime StartUhrzeit { get; set; }

        /// <summary>
        /// DateTime EndDatum
        /// </summary>
        [Required]
        public DateTime EndDatum { get; set; }

        /// <summary>
        ///  Guid Betrieb
        /// </summary>
        [Required(ErrorMessage = "Ein Betrieb ist erforderlich.")]
        public Guid Betrieb { get; set; }

        /// <summary>
        /// Guid SchlagID
        /// </summary>
        [Required(ErrorMessage = "Es muss eine Schlag ID angegben werden")]
        public Guid SchlagID { get; set; }

        /// <summary>
        /// string Duese 
        /// </summary>
        [Required]
        [MaxLength(50, ErrorMessage = "Bitte nicht mehr wie 50 Zeichen eingeben.")]
        public string Duese { get; set; }

        /// <summary>
        /// int WasseruhrAnfang 
        /// </summary>
        [Required(ErrorMessage = "Bitte den Anfangsstand der Wasseruhr angeben.")]
        public int WasseruhrAnfang { get; set; }

        /// <summary>
        /// virtual int WasseruhrEnde 
        /// </summary>
        public virtual int WasseruhrEnde { get; set; }

        /// <summary>
        /// string Vorkomnisse
        /// </summary>
        [MaxLength(100, ErrorMessage = "Bitte nicht mehr wie 100 Zeichen eingeben.")]
        public string Vorkomnisse { get; set; }

        /// <summary>
        /// virtual bool IstAbgeschlossen
        /// </summary>
        [Required(ErrorMessage = "Bitte angeben ob es noch in Arbeit ist oder abgeschlossen ist.")]
        public virtual bool IstAbgeschlossen { get; set; }
    }
}