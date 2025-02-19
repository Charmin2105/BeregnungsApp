﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REST.Api.Entities
{
    /// <summary>
    /// class Schlag
    /// </summary>
    public class Schlag
    {
        /// <summary>
        /// Guid ID
        /// </summary>
        [Key]
        public Guid ID { get; set; }

        /// <summary>
        /// string Name
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Betrieb 
        /// </summary>
        [ForeignKey("BetriebID")]
        public Betrieb Betrieb { get; set; }

        // Mögliche weitere Daten die erfasst werden sollen für einen Schlag
        //[Required]
        //public int Groesse { get; set; }
    }
}
