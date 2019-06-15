using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Models
{
    /// <summary>
    /// LinkDto
    /// </summary>
    public class LinkDto
    {
        /// <summary>
        /// string Href
        /// </summary>
        public string Href { get; private set; }
        /// <summary>
        /// string Rel
        /// </summary>
        public string Rel { get; private set; }
        /// <summary>
        /// string Method
        /// </summary>
        public string Method { get; private set; }

        /// <summary>
        /// LinkDto
        /// </summary>
        /// <param name="href">href zur Ressource</param>
        /// <param name="rel">Name der Anfrage</param>
        /// <param name="method">Methoden Typ</param>
        public LinkDto(string href, string rel, string method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }
    }
}
