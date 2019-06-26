using System;
using System.Collections.Generic;
using System.Text;

namespace Beregnungs.App.Models
{
    public class Schlag
    {
        /// <summary>
        /// Guid ID
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// string Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Guid BetriebID
        /// </summary>
        public Guid BetriebID { get; set; }


        public string BetriebIDString
        {
            get { return BetriebID.ToString(); }
            set { BetriebID = Guid.Parse(value); }
        }
        public string SchlagIDString
        {
            get { return ID.ToString(); }
            set { ID = Guid.Parse(value); }
        }
    }
}
