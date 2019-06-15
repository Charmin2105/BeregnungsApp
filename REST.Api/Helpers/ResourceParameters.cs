using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Helpers
{
    /// <summary>
    /// class ResourceParameters
    /// </summary>
    public abstract class ResourceParameters
    {
        /// <summary>
        /// string Fields
        /// </summary>
        public string Fields { get; set; }

        /// <summary>
        /// int maxPageSize
        /// </summary>
        const int maxPageSize = 20;

        /// <summary>
        /// int PageNumber
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        ///  int _pageSize
        /// </summary>
        private int _pageSize = 10;

        /// <summary>
        /// int PageSize
        /// </summary>
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {            
                //Check ob pageSize nicht maxSchlaegePageSize überschreitet
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
        /// <summary>
        /// string searchQuery
        /// </summary>
        public string searchQuery { get; set; }
    }
}
