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
            ////Setzt DB zurück
            context.BeregnungsDatens.RemoveRange(context.BeregnungsDatens);
            //context.Schlaege.RemoveRange(context.Schlaege);
            context.SaveChanges();

            //var schlaege = new List<Schlag>()
            //{
            //    new Schlag()
            //    {
            //        ID = new Guid("25320c5e-f58a-4b1f-b63a-8ee07a840bdf"),
            //        Name = "Feld 1"
            //    },
            //    new Schlag()
            //    {
            //        ID = new Guid("76053df4-6687-4353-8937-b45556748abe"),
            //        Name = "Feld 2"
            //    },
            //    new Schlag()
            //    {
            //        ID = new Guid("412c3012-d891-4f5e-9613-ff7aa63e6bb3"),
            //        Name = "Feld 3"
            //    },
            //};
            var daten = new List<BeregnungsDaten>()
            {
                new BeregnungsDaten()
                {
                    ID = new Guid("25320c5e-f58a-4b1f-b63a-8ee07a840bdf"),
                    StartDatum = new DateTimeOffset(new DateTime(2019, 5, 21)),
                    StartUhrzeit = DateTime.Today,
                    EndDatum = new DateTimeOffset(new DateTime(2019, 5, 23)),
                    Betrieb = new Guid("51004c54-3a86-4f55-b1a7-c6caeb8ca522"),
                    SchlagID = new Guid("51004c54-3a86-4f55-b1a7-c6caeb8ca532"),
                    Duese = "Düsenmaster 3000",
                    WasseruhrAnfang=0,
                    WasseruhrEnde=2000,
                    Vorkomnisse = "Keine",
                    IstAbgeschlossen= true
                },

                new BeregnungsDaten()
                {
                    ID = new Guid(),
                    StartDatum = new DateTimeOffset(new DateTime(2019, 5, 21)),
                    StartUhrzeit = DateTime.Today,
                    EndDatum = new DateTimeOffset(new DateTime(2019, 5, 23)),
                    Betrieb = new Guid("51004c54-3a86-4f55-b1a7-c6caeb8ca522"),
                    SchlagID = new Guid("51004c54-3a86-4f55-b1a7-c6caeb8ca532"),
                    Duese = "Düsenmaster 3000",
                    WasseruhrAnfang=0,
                    WasseruhrEnde=2000,
                    Vorkomnisse = "Keine",
                    IstAbgeschlossen= true
                },

            };

            context.BeregnungsDatens.AddRange(daten);
            //context.Schlaege.AddRange(context.Schlaege);
            context.SaveChanges();
        }
    }
}
