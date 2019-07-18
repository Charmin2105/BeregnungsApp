using System;

namespace Beregnungs.App.Models
{
    public class BeregnungsDaten
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
        ///  Wasseruhr Endestand
        /// </summary>
        public int Verbrauch
        {
            get { return WasseruhrEnde - WasseruhrAnfang; }
            set { value = WasseruhrEnde - WasseruhrAnfang; }
        }

        /// <summary>
        ///  Vorkomnisse
        /// </summary>
        public string Vorkomnisse { get; set; }

        /// <summary>
        ///  Ob die Beregnung abgeschlossen ist
        /// </summary>
        public bool IstAbgeschlossen { get; set; }

        public string StartDatumString
        {
            get { return StartDatum.ToString("d"); }
            set { StartDatum = DateTimeOffset.Parse(value); }
        }
        public TimeSpan Uhrzeit
        {
            get
            {
                return StartUhrzeit.TimeOfDay;
            }
            set
            {
                StartUhrzeit = DateTime.Parse(value.ToString());
            }
        }
        public string StartUhrzeitString
        {
            get { return StartUhrzeit.ToString("t"); }
            set { StartUhrzeit = DateTime.Parse(value); }
        }
        public string EndDatumString
        {
            get { return EndDatum.ToString("d"); }
            set { EndDatum = DateTimeOffset.Parse(value); }
        }
        public string BetriebIDString
        {
            get { return BetriebID.ToString(); }
            set { BetriebID = Guid.Parse(value); }
        }
        public string SchlagIDString
        {
            get { return SchlagID.ToString(); }
            set { SchlagID = Guid.Parse(value); }
        }
    }
}