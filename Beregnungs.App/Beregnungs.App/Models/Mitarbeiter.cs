using System;
using System.Collections.Generic;
using System.Text;

namespace Beregnungs.App.Models
{
    public class Mitarbeiter
    {
        /// <summary>
        /// Guid ID
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// string Vorname
        /// </summary>
        public string Vorname { get; set; }

        /// <summary>
        /// string Nachname
        /// </summary>
        public string Nachname { get; set; }

        /// <summary>
        ///  DateTimeOffset Geburtstag
        /// </summary>
        public DateTimeOffset Geburtstag { get; set; }

        /// <summary>
        /// Guid BetriebID
        /// </summary>
        public Guid BetriebID { get; set; }

        public string BetriebIDString
        {
            get { return BetriebID.ToString(); }
            set { BetriebID = Guid.Parse(value); }
        }

        public string IDString
        {
            get { return ID.ToString(); }
        }

        public string GebDatumString
        {
            get { return Geburtstag.ToString(); }
        }
    }
}
