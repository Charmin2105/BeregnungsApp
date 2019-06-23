using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Entities
{
    /// <summary>
    /// Betrieb
    /// </summary>
    public class Betrieb
    {
        /// <summary>
        /// Guid ID
        /// </summary>
        [Key]
        public Guid ID { get; set; }

        /// <summary>
        /// string Name
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// ICollection<Mitarbeiter> Mitarbeiters
        /// </summary>
        public ICollection<Mitarbeiter> Mitarbeiters { get; set; }
            = new List<Mitarbeiter>();

        /// <summary>
        /// ICollection<Mitarbeiter> Mitarbeiters
        /// </summary>
        public ICollection<BeregnungsDaten> BeregnungsDaten { get; set; }
            = new List<BeregnungsDaten>();
    }
}
