using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Models
{
    /// <summary>
    /// class BeregnungsDatenDto
    /// </summary>
    public class BeregnungsDatenDto
    {
        /// <summary>
        ///  ID 
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        ///  Start Datum der Beregnung
        /// </summary>
        public DateTimeOffset StartDatum { get; set; }

        /// <summary>
        ///  Start Uhrzeit der Beregnung
        /// </summary>
        public DateTime StartUhrzeit { get; set; }

        /// <summary>
        ///  End Datum der Beregnung
        /// </summary>
        public DateTimeOffset EndDatum { get; set; }

        /// <summary>
        ///   ID des Betriebs der die Beregnung durchgeführt hat
        /// </summary>
        public Guid BetriebID { get; set; }

        /// <summary>
        ///  ID des Schlages  
        /// </summary>
        public Guid SchlagID { get; set; }

        /// <summary>
        ///  Duese die verwendet wurde
        /// </summary>
        public string Duese { get; set; }

        /// <summary>
        ///  Wasseruhr Anfangsstand
        /// </summary>
        public int WasseruhrAnfang { get; set; }

        /// <summary>
        ///  Wasseruhr Endestand
        /// </summary>
        public int WasseruhrEnde { get; set; }

        /// <summary>
        ///  Vorkomnisse
        /// </summary>
        public string Vorkomnisse { get; set; }

        /// <summary>
        ///  Ob die Beregnung abgeschlossen ist
        /// </summary>
        public bool IstAbgeschlossen { get; set; }

    }
}
