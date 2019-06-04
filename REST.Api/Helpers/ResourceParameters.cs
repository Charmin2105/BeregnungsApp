using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Helpers
{
    public class ResourceParameters
    {
        const int maxPageSize = 20;

        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;

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
    }
}
