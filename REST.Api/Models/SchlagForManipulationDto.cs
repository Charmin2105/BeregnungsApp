using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Models
{
    public abstract class SchlagForManipulationDto
    {
        [Required(ErrorMessage = "Ein Name ist erforderlich.")]
        [MaxLength(100, ErrorMessage = "Bitte nicht mehr wie 100 Zeichen eingeben.")]
        public virtual string Name { get; set; }
    }
}
