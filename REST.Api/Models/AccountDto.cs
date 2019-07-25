using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Models
{
    public class AccountDto : LinkedResourceBaseDto
    {
        /// <summary>
        /// Guid ID
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// string Benutzernamen
        /// </summary>
        public string Benutzername { get; set; }

        /// <summary>
        /// string Benutzernamen
        /// </summary>
        public string EMail { get; set; }

        /// <summary>
        /// string Passwort
        /// </summary>
        public string Passwort { get; set; }

        /// <summary>
        /// bool IstAdmin
        /// </summary>
        public bool IstAdmin { get; set; }

        /// <summary>
        /// Guid BetriebID
        /// </summary>
        public Guid BetriebID { get; set; }
    }
}
