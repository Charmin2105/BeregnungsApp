using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Models
{
    public class BeregnungsDatenForUpdateDto : BeregnungsDatenForManipulationDto
    {
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
