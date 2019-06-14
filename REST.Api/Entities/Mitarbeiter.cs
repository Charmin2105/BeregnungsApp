using System;
using System.ComponentModel.DataAnnotations;

namespace REST.Api.Entities
{
    public class Mitarbeiter
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Vorname { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nachname { get; set; }

        [Required]
        public DateTimeOffset Geburtstag { get; set; }

        [Required]
        public Guid BetriebID { get; set; }
        

    }
}