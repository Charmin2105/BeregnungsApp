using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Models
{
    /// <summary>
    /// class BetriebForCreationDto 
    /// </summary>
    public class BetriebForCreationDto : BetriebForMaipulationDto
    {
        /// <summary>
        /// Mitarbeiters
        /// </summary>
        public ICollection<MitarbeiterForCreationDto> Mitarbeiters { get; set; }
            = new List<MitarbeiterForCreationDto>();
    }
}
