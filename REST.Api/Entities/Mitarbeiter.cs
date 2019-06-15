using System;
using System.ComponentModel.DataAnnotations;

namespace REST.Api.Entities
{
    /// <summary>
    /// Mitarbeiter
    /// </summary>
    public class Mitarbeiter
    {
        /// <summary>
        /// Guid ID
        /// </summary>
        [Key]
        public Guid ID { get; set; }

        /// <summary>
        /// string Vorname
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Vorname { get; set; }

        /// <summary>
        /// string Nachname
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Nachname { get; set; }

        /// <summary>
        ///  DateTimeOffset Geburtstag
        /// </summary>
        [Required]
        public DateTimeOffset Geburtstag { get; set; }

        /// <summary>
        /// Guid BetriebID
        /// </summary>
        [Required]
        public Guid BetriebID { get; set; }
        

    }
}