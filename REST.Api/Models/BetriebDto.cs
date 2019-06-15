using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Models
{
    /// <summary>
    /// class BetriebDto
    /// </summary>
    public class BetriebDto
    {
        /// <summary>
        ///  ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        ///  Name des Betriebs 
        /// </summary>
        public string Name { get; set; }
    }
}
