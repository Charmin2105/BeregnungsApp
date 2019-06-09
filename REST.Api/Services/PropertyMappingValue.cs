using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Services
{
    public class PropertyMappingValue
    {
        public IEnumerable<string> DestinationPropertise { get; private set; }

        public bool Revert { get; private set; }

        public PropertyMappingValue(IEnumerable<string> destinationProperties, bool revert = false)
        {
            DestinationPropertise = destinationProperties;
            Revert = revert;
        }
    }
}
