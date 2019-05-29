using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeregnungsRESTapi.Entities
{
    public class Beregnungs
    {

        [Key]
        public Guid ID { get; set; }

        [Required]
        public DateTime StartDatum { get; set; }
        
        [MaxLength(50)]
        public string StartUhrzeit { get; set; }

        [Required]
        public DateTime EndeDatum { get; set; }

        [Required]
        [MaxLength(50)]
        public string Schlag { get; set; }

        [Required]
        [MaxLength(50)]
        public string Duese { get; set; }

        [Required]
        [MaxLength(50)]
        public int WasseruhrAnfang { get; set; }

        [Required]
        [MaxLength(50)]
        public int WasseruhrEnde { get; set; }

        [Required]
        [MaxLength(50)]
        public string Vorkommnisse { get; set; }

    }
}
