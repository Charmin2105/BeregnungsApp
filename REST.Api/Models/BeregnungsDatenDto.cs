using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Models
{
    public class BeregnungsDatenDto
    {
        public Guid ID { get; set; }

        public DateTimeOffset StartDatum { get; set; }

        public DateTime StartUhrzeit { get; set; }

        public DateTimeOffset EndDatum { get; set; }

        public string Betrieb { get; set; }

        public Guid SchlagID { get; set; }

        public string Duese { get; set; }

        public int WasseruhrAnfang { get; set; }

        public int WasseruhrEnde { get; set; }

        public string Vorkomnisse { get; set; }

        public bool IstAbgeschlossen { get; set; }

    }
}
