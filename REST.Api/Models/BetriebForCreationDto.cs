using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Models
{
    public class BetriebForCreationDto : BetriebForMaipulationDto
    {

        public ICollection<MitarbeiterForCreationDto> Books { get; set; }
            = new List<MitarbeiterForCreationDto>();
    }
}
