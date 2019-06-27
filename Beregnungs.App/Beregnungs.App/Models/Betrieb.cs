using System;
using System.Collections.Generic;
using System.Text;

namespace Beregnungs.App.Models
{
    public class Betrieb
    {
        /// <summary>
        /// Guid ID
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// string Name
        /// </summary>
        public string Name { get; set; }

        public string BetriebIDString
        {
            get { return ID.ToString(); }
            set { ID = Guid.Parse(value); }
        }
    }
}
