using Microsoft.EntityFrameworkCore;

namespace BeregnungsRESTapi.Entities
{
    public class BeregnungsContext : DbContext
    {
        public BeregnungsContext(DbContextOptions<BeregnungsContext> options) 
            : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Beregnungs> Beregnungsdaten { get; set; }
    }
}
