using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Helpers
{
    /// <summary>
    /// BeregnungsDatenResourceParameter
    /// </summary>
    public class BeregnungsDatenResourceParameter : ResourceParameters
    {
        /// <summary>
        /// string IstAbgeschlossen
        /// </summary>
        public string IstAbgeschlossen { get; set; }

        /// <summary>
        /// Guid SchlagId
        /// </summary>
        public Guid SchlagId { get; set; }

        /// <summary>
        /// string OrderBy
        /// </summary>
        public string OrderBy { get; set; } = "StartDatum";
    }
}
