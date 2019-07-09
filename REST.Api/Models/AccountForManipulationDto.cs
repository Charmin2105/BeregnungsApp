using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Models
{
    public class AccountForManipulationDto
    {
        /// <summary>
        /// string Benutzernamen
        /// </summary>
        [Required(ErrorMessage = "Ein Benutzernamen ist erforderlich.")]
        public string Benutzername { get; set; }

        /// <summary>
        /// string Benutzernamen
        /// </summary>
        [Required(ErrorMessage = "Eine E-Mail Adresse ist erforderlich.")]
        public string EMail { get; set; }

        /// <summary>
        /// string Passwort
        /// </summary>
        [Required(ErrorMessage = "Ein Passwort ist erforderlich.")]
        public string Passwort { get; set; }

        /// <summary>
        /// bool IstAdmin
        /// </summary>
        [Required(ErrorMessage = "Bitte angeben ob der Account Administrationsrecht besitzt.")]
        public bool IstAdmin { get; set; }
    }
}
