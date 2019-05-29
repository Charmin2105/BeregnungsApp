using System;

namespace BeregnungsRESTapi.Models
{
    internal class BergnungsDto
    {
        public Guid ID { get; set; }
        public DateTime StartDatum { get; set; }
        public string StartUhrzeit { get; set; }
        public DateTime EndeDatum { get; set; }
        public string Schlag { get; set; }
        public string Duese { get; set; }
        public int WasseruhrAnfang { get; set; }
        public int WasseruhrEnde { get; set; }
        public string Vorkommnisse { get; set; }
    }
}