using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Entities
{
    /// <summary>
    /// BeregnungsContext
    /// </summary>
    public class BeregnungsContext : DbContext
    {
        /// <summary>
        /// BeregnungsContext
        /// </summary>
        /// <param name="options">DbContextOptions<BeregnungsContext></param>
        public BeregnungsContext(DbContextOptions<BeregnungsContext> options)
            : base(options)
        {
            Database.Migrate();
        }
        /// <summary>
        /// BeregnungsDatens
        /// </summary>
        public DbSet<BeregnungsDaten> BeregnungsDatens { get; set; }
        /// <summary>
        /// Schlaege
        /// </summary>
        public DbSet<Schlag> Schlaege { get; set; }
        /// <summary>
        /// Betriebe
        /// </summary>
        public DbSet<Betrieb> Betriebe { get; set; }
        /// <summary>
        /// Mitarbeiters
        /// </summary>
        public DbSet<Mitarbeiter> Mitarbeiters { get; set; }
    }

}
