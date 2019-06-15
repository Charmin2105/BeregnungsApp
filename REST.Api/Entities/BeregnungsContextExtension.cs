using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Entities
{
    /// <summary>
    /// BeregnungsContextExtension
    /// Zum Testen erstmalig Daten in DB 
    /// </summary>
    public static class BeregnungsContextExtension
    {
        /// <summary>
        /// DataForContext
        /// </summary>
        /// <param name="context">BeregnungsContext</param>
        public static void DataForContext(this BeregnungsContext context)
        {
            ////Setzt DB zurück
            //context.BeregnungsDatens.RemoveRange(context.BeregnungsDatens);
            //context.Schlaege.RemoveRange(context.Schlaege);
            context.Betriebe.RemoveRange(context.Betriebe);
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
            var betrieb = new List<Betrieb>()
            {
                new Betrieb()
                {
                    ID = new Guid("25320c5e-f58a-4b1f-b63a-8ee07a840bdf"),
                    Name = "SudiFarm",
                    Mitarbeiters = new List<Mitarbeiter>()
                    {
                        new Mitarbeiter()
                        {
                            ID = new Guid(),
                            Vorname = "Dennis",
                            Nachname = "Welzer",
                            Geburtstag = new DateTimeOffset(new DateTime(1995,1,22))
                        },
                        new Mitarbeiter()
                        {
                            ID = new Guid(),
                            Vorname = "Benjamin",
                            Nachname = "Schmidt",
                            Geburtstag = new DateTimeOffset(new DateTime(1996,1,10))
                        },
                        new Mitarbeiter()
                        {
                            ID = new Guid(),
                            Vorname = "Marc",
                            Nachname = "Nachname",
                            Geburtstag = new DateTimeOffset(new DateTime(1996,11,23))
                        },
                        new Mitarbeiter()
                        {
                            ID = new Guid(),
                            Vorname = "Hendrik",
                            Nachname = "Klingenberg",
                            Geburtstag = new DateTimeOffset(new DateTime(1992,04,30))
                        }
                    }
                },
                new Betrieb()
                {
                    ID = new Guid(),
                    Name = "Maya Bauernhof",
                    Mitarbeiters = new List<Mitarbeiter>()
                    {
                        new Mitarbeiter()
                        {
                            ID = new Guid(),
                            Vorname = "Uwe",
                            Nachname = "Hackbarth",
                            Geburtstag = new DateTimeOffset(new DateTime(1955,3,29))
                        },
                        new Mitarbeiter()
                        {
                            ID = new Guid(),
                            Vorname = "Timo",
                            Nachname = "Hackbarth",
                            Geburtstag = new DateTimeOffset(new DateTime(1980,4,2))
                        },
                     }
                }
            };
            //var daten = new List<BeregnungsDaten>()
            //{
            //    new BeregnungsDaten()
            //    {
            //        ID = new Guid("25320c5e-f58a-4b1f-b63a-8ee07a840bdf"),
            //        StartDatum = new DateTimeOffset(new DateTime(2019, 5, 21)),
            //        StartUhrzeit = DateTime.Today,
            //        EndDatum = new DateTimeOffset(new DateTime(2019, 5, 23)),
            //        Betrieb = new Guid("51004c54-3a86-4f55-b1a7-c6caeb8ca522"),
            //        SchlagID = new Guid("51004c54-3a86-4f55-b1a7-c6caeb8ca532"),
            //        Duese = "Düsenmaster 3000",
            //        WasseruhrAnfang=0,
            //        WasseruhrEnde=2000,
            //        Vorkomnisse = "Keine",
            //        IstAbgeschlossen= true
            //    },

            //    new BeregnungsDaten()
            //    {
            //        ID = new Guid(),
            //        StartDatum = new DateTimeOffset(new DateTime(2019, 5, 21)),
            //        StartUhrzeit = DateTime.Today,
            //        EndDatum = new DateTimeOffset(new DateTime(2019, 5, 23)),
            //        Betrieb = new Guid("51004c54-3a86-4f55-b1a7-c6caeb8ca522"),
            //        SchlagID = new Guid("51004c54-3a86-4f55-b1a7-c6caeb8ca532"),
            //        Duese = "Düsenmaster 3000",
            //        WasseruhrAnfang=0,
            //        WasseruhrEnde=2000,
            //        Vorkomnisse = "Keine",
            //        IstAbgeschlossen= true
            //    },

            //};

            //context.BeregnungsDatens.AddRange(daten);
            //context.Schlaege.AddRange(context.Schlaege);
            context.Betriebe.AddRange(betrieb);
            context.SaveChanges();
        }
    }
}
