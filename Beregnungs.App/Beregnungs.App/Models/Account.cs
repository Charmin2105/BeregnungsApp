using System;
using System.Collections.Generic;
using System.Text;

namespace Beregnungs.App.Models
{
    public class Account
    {

        /// <summary>
        /// string Benutzernamen
        /// </summary>
        public string Benutzername { get; set; }

        /// <summary>
        /// string Passwort
        /// </summary>
        public string Passwort { get; set; }

        /// <summary>
        /// Guid BetriebID
        /// </summary>
        public Guid BetriebID { get; set; }

    }
}
