using System;

namespace REST.Api.Models
{
    /// <summary>
    /// class SchlagDto
    /// </summary>
    public class SchlagDto : LinkedResourceBaseDto
    {
        /// <summary>
        ///  ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        ///  Name des Schlags
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Guid BetriebID
        /// </summary>
        public Guid BetriebID { get; set; }
    }
}