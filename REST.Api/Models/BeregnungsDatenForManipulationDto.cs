using System;
using System.ComponentModel.DataAnnotations;

namespace REST.Api.Models
{
    public abstract class BeregnungsDatenForManipulationDto
    {
        [Required(ErrorMessage = "Ein Startdatum ist erforderlich.")]
        public DateTime StartDatum { get; set; }

        [Required(ErrorMessage = "Ein Uhrzeit ist erforderlich.")]
        public DateTime StartUhrzeit { get; set; }

        [Required]
        public DateTime EndDatum { get; set; }

        [Required(ErrorMessage = "Ein Betrieb ist erforderlich.")]
        public string Betrieb { get; set; }

        [Required(ErrorMessage = "Es muss eine Schlag ID angegben werden")]

        public Guid SchlagID { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Bitte nicht mehr wie 50 Zeichen eingeben.")]
        public string Duese { get; set; }

        [Required(ErrorMessage = "Bitte den Anfangsstand der Wasseruhr angeben.")]
        public int WasseruhrAnfang { get; set; }

        public virtual int WasseruhrEnde { get; set; }

        [MaxLength(100, ErrorMessage = "Bitte nicht mehr wie 100 Zeichen eingeben.")]
        public string Vorkomnisse { get; set; }

        [Required(ErrorMessage = "Bitte angeben ob es noch in Arbeit ist oder abgeschlossen ist.")]
        public virtual bool IstAbgeschlossen { get; set; }
    }
}