using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Models
{
    /// <summary>
    /// BetriebForMaipulationDto
    /// </summary>
    public abstract class BetriebForMaipulationDto
    {
        /// <summary>
        ///   Name des Betrieb
        /// </summary>
        [Required(ErrorMessage = "Es ist ein Name erforderlich.")]
        public virtual string Name { get; set; }
    }
}
