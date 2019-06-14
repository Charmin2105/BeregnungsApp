using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Models
{
    public class MitarbeiterDto : LinkedResourceBaseDto
    {
        public Guid ID { get; set; }

        public string Vorname { get; set; }

        public string Nachname { get; set; }

        public DateTimeOffset GebDatum { get; set; }
    }
}
