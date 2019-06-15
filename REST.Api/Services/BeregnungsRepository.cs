using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using REST.Api.Entities;
using REST.Api.Helpers;
using REST.Api.Models;

namespace REST.Api.Services
{
    /// <summary>
    /// Daten verarbeiten
    /// </summary>
    public class BeregnungsRepository : IBeregnungsRepository
    {

        private BeregnungsContext _context;
        private IPropertyMappingService _propertyMappingService;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Datenbank Inhalt</param>
        public BeregnungsRepository(BeregnungsContext context, IPropertyMappingService propertyMappingService)
        {
            _context = context;
            _propertyMappingService = propertyMappingService;
        }
        /// <summary>
        /// Hinzufügen neuer Daten
        /// </summary>
        /// <param name="daten">Neue Daten</param>
        public void AddBeregnungsDaten(BeregnungsDaten daten)
        {
            daten.ID = Guid.NewGuid();
            _context.BeregnungsDatens.Add(daten);
            
        }

        /// <summary>
        /// Abfrage ob eine bestimmte Daten existieren
        /// </summary>
        /// <param name="guid">Abzufragende Daten</param>
        /// <returns>bool</returns>
        public bool BeregnungsDatenExists(Guid guid)
        {
            return _context.BeregnungsDatens.Any(a => a.ID == guid);
        }

        /// <summary>
        /// Daten löschen
        /// </summary>
        /// <param name="daten">Zu löschende Daten</param>
        public void DeleteBeregnungsDaten(BeregnungsDaten daten)
        {
            _context.BeregnungsDatens.Remove(daten);
        }

        /// <summary>
        /// Eine bestimmte Daten anzeigen
        /// </summary>
        /// <param name="guid">ID des Daten</param>
        /// <returns></returns>
        public BeregnungsDaten GetBeregnungsDaten(Guid guid)
        {
            return _context.BeregnungsDatens.FirstOrDefault(a => a.ID == guid);
        }

        /// <summary>
        /// GetDaten mit Seiten
        /// </summary>
        /// <param name="datenresourceParameters">Seiteneinstellungen</param>
        /// <returns> PagedList<Daten></returns>
        public PagedList<BeregnungsDaten> GetBeregnungsDatens(BeregnungsDatenResourceParameter datenresourceParameters)
        {
            //var collectionBeforPaging = _context.BeregnungsDatens.OrderBy(a =>
            //a.StartDatum).AsQueryable();
            var collectionBeforPaging = 
                _context.BeregnungsDatens.ApplySort(datenresourceParameters.OrderBy,
                _propertyMappingService.GetPropertyMapping<BeregnungsDatenDto,BeregnungsDaten>());

            // Filter nach abgeschlossenen Daten
            // Nicht Filter sondern Suche ist hier gefordert
            //var abgeschlossenForWhereClause = datenresourceParameters.IstAbgeschlossen;

            //collectionBeforPaging = collectionBeforPaging
            //    .Where(a =>
            //    a.IstAbgeschlossen == abgeschlossenForWhereClause&!abgeschlossenForWhereClause);

            //Filter nach SchlagId
            if (datenresourceParameters.SchlagId != new Guid("00000000-0000-0000-0000-000000000000"))
            {
                var schlagIdForWhereClause = datenresourceParameters.SchlagId;
                collectionBeforPaging = collectionBeforPaging.Where(a =>
                a.SchlagID == schlagIdForWhereClause);
            }

            //Suche nach abgeschlossenen Daten
            if (!string.IsNullOrEmpty(datenresourceParameters.IstAbgeschlossen))
            {                
                var abgeschlossenForWhereClause = bool.Parse(datenresourceParameters.IstAbgeschlossen);
                collectionBeforPaging = collectionBeforPaging.Where(a =>
                a.IstAbgeschlossen == abgeschlossenForWhereClause);
            }


            return PagedList<BeregnungsDaten>.Create(collectionBeforPaging, datenresourceParameters.PageNumber, datenresourceParameters.PageSize);
        }

        /// <summary>
        /// GetDaten
        /// </summary>
        /// <param name="guids">IEnumerable der IDs</param>
        /// <returns>IEnumerable</returns>
        public IEnumerable<BeregnungsDaten> GetBeregnungsDatens(IEnumerable<Guid> guids)
        {
            return _context.BeregnungsDatens.Where(a => guids.Contains(a.ID)).ToList();
        }

        /// <summary>
        /// Abfrage ob gespeichert wurde
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateBeregnungsDaten(BeregnungsDaten daten)
        {
           // throw new NotImplementedException();
        }
    }
}
