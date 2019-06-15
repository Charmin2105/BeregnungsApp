using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Models
{
    /// <summary>
    /// BetriebForUpdateDto
    /// </summary>
    public class BetriebForUpdateDto : BetriebForMaipulationDto
    {
        /// <summary>
        /// override string Name
        /// </summary>
        [Required(ErrorMessage = "Ein Name ist erforderlich.")]
        public override string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                base.Name = value;
            }
        }
    }
}
