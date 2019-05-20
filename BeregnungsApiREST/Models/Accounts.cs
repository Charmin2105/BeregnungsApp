using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeregnungsApiREST.Models
{
    public class Accounts
    {
        public long Id { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
    }
}
