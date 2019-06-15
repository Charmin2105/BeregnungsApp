using System;
using System.ComponentModel.DataAnnotations;

namespace REST.Api.Entities
{
    /// <summary>
    /// BeregnungsDaten
    /// </summary>
    public class BeregnungsDaten
    {
        /// <summary>
        /// Guid ID
        /// </summary>
        [Key]
        public Guid ID { get; set; }

        /// <summary>
        /// DateTimeOffset StartDatum
        /// </summary>
        [Required]
        public DateTimeOffset StartDatum { get; set; }

        /// <summary>
        /// DateTime StartUhrzeit 
        /// </summary>
        public DateTime StartUhrzeit { get; set; }

        /// <summary>
        /// DateTimeOffset EndDatum
        /// </summary>
        [Required]
        public DateTimeOffset EndDatum { get; set; }

        /// <summary>
        /// Guid Betrieb 
        /// </summary>
        [Required]
        public Guid Betrieb { get; set; }

        /// <summary>
        /// Guid SchlagID
        /// </summary>
        [Required]
        [MaxLength(50)]
        public Guid SchlagID { get; set; }

        /// <summary>
        /// string Duese
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Duese { get; set; }

        /// <summary>
        /// int WasseruhrAnfang
        /// </summary>
        [Required]
        public int WasseruhrAnfang { get; set; }

        /// <summary>
        /// int WasseruhrEnde
        /// </summary>
        [Required]
        public int WasseruhrEnde { get; set; }

        /// <summary>
        /// string Vorkomnisse
        /// </summary>
        [MaxLength(50)]
        public string Vorkomnisse { get; set; }

        /// <summary>
        /// bool IstAbgeschlossen
        /// </summary>
        [Required]
        public bool IstAbgeschlossen { get; set; }

    }
}
