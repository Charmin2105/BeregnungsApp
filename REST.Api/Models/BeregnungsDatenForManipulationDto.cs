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
        /// Start Datum der Beregnung
        /// </summary>
        [Required(ErrorMessage = "Ein Startdatum ist erforderlich.")]
        public DateTime StartDatum { get; set; }

        /// <summary>
        /// Start Uhrzeit der Beregnung
        /// </summary>
        [Required(ErrorMessage = "Ein Uhrzeit ist erforderlich.")]
        public DateTime StartUhrzeit { get; set; }

        /// <summary>
        /// End Datum der Beregnung
        /// </summary>
        [Required]
        public DateTime EndDatum { get; set; }

        /// <summary>
        ///  Betrieb der die Beregnung durchgeführt hat
        /// </summary>
        [Required(ErrorMessage = "Ein Betrieb ist erforderlich.")]
        public Guid Betrieb { get; set; }

        /// <summary>
        /// Schlag der Beregnet wurde
        /// </summary>
        [Required(ErrorMessage = "Es muss eine Schlag ID angegben werden")]
        public Guid SchlagID { get; set; }

        /// <summary>
        ///  Duese die Verwendet wurde
        /// </summary>
        [Required]
        [MaxLength(50, ErrorMessage = "Bitte nicht mehr wie 50 Zeichen eingeben.")]
        public string Duese { get; set; }

        /// <summary>
        /// Wasseruhr Anfangstand
        /// </summary>
        [Required(ErrorMessage = "Bitte den Anfangsstand der Wasseruhr angeben.")]
        public int WasseruhrAnfang { get; set; }

        /// <summary>
        /// Wasseruhr Endestand
        /// </summary>
        public virtual int WasseruhrEnde { get; set; }

        /// <summary>
        /// Vorkomnisse die Aufgetreten sind
        /// </summary>
        [MaxLength(100, ErrorMessage = "Bitte nicht mehr wie 100 Zeichen eingeben.")]
        public string Vorkomnisse { get; set; }

        /// <summary>
        /// Ist Abgeschlossen Beregnung
        /// </summary>
        [Required(ErrorMessage = "Bitte angeben ob es noch in Arbeit ist oder abgeschlossen ist.")]
        public virtual bool IstAbgeschlossen { get; set; }
    }
}