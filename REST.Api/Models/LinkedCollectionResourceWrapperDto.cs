using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Models
{
    /// <summary>
    /// LinkedCollectionResourceWrapperDto
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LinkedCollectionResourceWrapperDto<T> : LinkedResourceBaseDto 
        where T : LinkedResourceBaseDto
    {
        public IEnumerable<T> Value { get; set; }

        /// <summaryLinkedCollectionResourceWrapperDto>
        /// 
        /// </summary>
        /// <param name="value">value</param>
        public LinkedCollectionResourceWrapperDto(IEnumerable<T> value)
        {
            Value = value;
        }
    }
}
