using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Models
{
    /// <summary>
    /// MitarbeiterDto
    /// </summary>
    public class MitarbeiterDto : LinkedResourceBaseDto
    {
        /// <summary>
        ///  ID 
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// Vorname des Mitarbeiters
        /// </summary>
        public string Vorname { get; set; }

        /// <summary>
        ///  Nachname des Mitarbeiters
        /// </summary>
        public string Nachname { get; set; }

        /// <summary>
        ///  GebDatum des Mitarbeiters
        /// </summary>
        public DateTimeOffset Geburtstag { get; set; }
    }
}
