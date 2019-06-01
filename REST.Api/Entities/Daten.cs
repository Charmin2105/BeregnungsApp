using System;
using System.ComponentModel.DataAnnotations;

namespace REST.Api.Entities
{
    public class Daten
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        public DateTime StartDatum { get; set; }

        public DateTime StartUhrzeit { get; set; }

        [Required]
        public DateTime EndDatum { get; set; }

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

        [Required]
        [MaxLength(50)]
        public string Vorkomnisse { get; set; }

    }
}
