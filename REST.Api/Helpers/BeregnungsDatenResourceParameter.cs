using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Helpers
{
    public class BeregnungsDatenResourceParameter : ResourceParameters
    {
        public string IstAbgeschlossen { get; set; }
        public Guid SchlagId { get; set; }
        public string OrderBy { get; set; } = "StartDatum";
        public string Fields { get; set; }
    }
}
