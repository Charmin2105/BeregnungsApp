using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Entities
{
    public class BeregnungsContext : DbContext
    {
        public BeregnungsContext(DbContextOptions<BeregnungsContext> options)
            : base(options)
        {
            Database.Migrate();
        }
        public DbSet<BeregnungsDaten> BeregnungsDatens { get; set; }
        public DbSet<Schlag> Schlaege { get; set; }
        public DbSet<Betrieb> Betriebe { get; set; }
    }

}
