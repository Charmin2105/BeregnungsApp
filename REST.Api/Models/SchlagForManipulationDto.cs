using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Models
{
    /// <summary>
    ///  class SchlagForManipulationDto
    /// </summary>
    public abstract class SchlagForManipulationDto
    {
        /// <summary>
        /// Name des Schlags
        /// </summary>
        [Required(ErrorMessage = "Ein Name ist erforderlich.")]
        [MaxLength(100, ErrorMessage = "Bitte nicht mehr wie 100 Zeichen eingeben.")]
        public virtual string Name { get; set; }
    }
}
