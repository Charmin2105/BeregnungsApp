using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Models
{
    /// <summary>
    /// LinkedResourceBaseDto
    /// </summary>
    public abstract class LinkedResourceBaseDto
    {
        /// <summary>
        /// List<LinkDto> Links
        /// </summary>
        public List<LinkDto> Links { get; set; } = new List<LinkDto>();
    }
}
