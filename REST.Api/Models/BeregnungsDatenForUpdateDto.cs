using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Models
{
    /// <summary>
    /// BeregnungsDatenForUpdateDto
    /// </summary>
    public class BeregnungsDatenForUpdateDto : BeregnungsDatenForManipulationDto
    {
        /// <summary>
        /// Wasseruhr Ende
        /// </summary>
        [Required(ErrorMessage = "Bitte den finalen Wasseruhrstand angeben")]
        public override int WasseruhrEnde
        {
            get
            {
                return base.WasseruhrEnde;
            }
            set
            {
                base.WasseruhrEnde = value;
            }
        }

        /// <summary>
        /// Ist Abgeschlossen
        /// </summary>
        [Required(ErrorMessage = "Bitte angeben ob es noch in Arbeit ist oder abgeschlossen ist.")]
        public override bool IstAbgeschlossen
        {
            get
            {
                return base.IstAbgeschlossen;
            }
            set
            {
                base.IstAbgeschlossen = value;
            }
        }
    }
}
