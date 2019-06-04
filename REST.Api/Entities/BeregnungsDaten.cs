using System;
using System.ComponentModel.DataAnnotations;

namespace REST.Api.Entities
{
    public class BeregnungsDaten
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        public DateTimeOffset StartDatum { get; set; }

        public DateTime StartUhrzeit { get; set; }

        [Required]
        public DateTimeOffset EndDatum { get; set; }

        [Required]
        public string Betrieb { get; set; }

        [Required]
        [MaxLength(50)]
        public Guid SchlagID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Duese { get; set; }

        [Required]
        public int WasseruhrAnfang { get; set; }

        [Required]
        public int WasseruhrEnde { get; set; }

        [MaxLength(50)]
        public string Vorkomnisse { get; set; }

        [Required]
        public bool IstAbgeschlossen { get; set; }

    }
}
