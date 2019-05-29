using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Entities
{
    public static class BeregnungsContextExtension
    {
        public static void DataForContext(this BeregnungsContext context)
        {
            //Setzt DB zurück
            context.Schlaege.RemoveRange(context.Schlaege);
            context.SaveChanges();


            var schlaege = new List<Schlag>()
            {
                new Schlag()
                {
                    ID = new Guid("25320c5e-f58a-4b1f-b63a-8ee07a840bdf"),
                    Name = "Feld 1"                    
                },
                new Schlag()
                {
                    ID = new Guid("76053df4-6687-4353-8937-b45556748abe"),
                    Name = "Feld 2"
                },
                new Schlag()
                {
                    ID = new Guid("412c3012-d891-4f5e-9613-ff7aa63e6bb3"),
                    Name = "Feld 3"
                },
            };

            context.Schlaege.AddRange(schlaege);
            context.SaveChanges();
        }
    }
}
