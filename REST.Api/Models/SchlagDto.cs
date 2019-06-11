using System;

namespace REST.Api.Models
{
    public class SchlagDto : LinkedResourceBaseDto
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }
}