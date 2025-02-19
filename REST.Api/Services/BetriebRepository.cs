﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using REST.Api.Entities;
using REST.Api.Helpers;
using REST.Api.Models;

namespace REST.Api.Services
{
    /// <summary>
    /// BetriebRepository
    /// </summary>
    public class BetriebRepository : IBetriebRepository
    {
        private BeregnungsContext _context;
        private IPropertyMappingService _propertyMappingService;

        #region Betrieb
        /// <summary>
        /// BetriebRepository
        /// </summary>
        /// <param name="context">BeregnungsContext</param>
        public BetriebRepository(BeregnungsContext context, IPropertyMappingService propertyMappingService)
        {
            _propertyMappingService = propertyMappingService;
            _context = context;
        }

        /// <summary>
        /// AddBetrieb
        /// </summary>
        /// <param name="betrieb">Betrieb</param>
        public void AddBetrieb(Betrieb betrieb)
        {
            betrieb.ID = Guid.NewGuid();
            _context.Betriebe.Add(betrieb);
        }

        /// <summary>
        /// DeleteBetrieb
        /// </summary>
        /// <param name="betrieb">Betrieb</param>
        public void DeleteBetrieb(Betrieb betrieb)
        {
            _context.Betriebe.Remove(betrieb);
        }

        /// <summary>
        /// GetBetrieb
        /// </summary>
        /// <param name="id">Guid</param>
        /// <returns></returns>
        public Betrieb GetBetrieb(Guid id)
        {
            return _context.Betriebe.FirstOrDefault(a => a.ID == id);
        }

        /// <summary>
        /// GetBetriebe
        /// </summary>
        /// <param name="resourcePrameter">BetriebResourceParameter</param>
        /// <returns>PagedList<Betrieb></returns>
        public PagedList<Betrieb> GetBetriebe(BetriebResourceParameter resourcePrameter)
        {
            var collectionBeforePageing = _context.Betriebe.OrderBy(a => a.Name);
            return PagedList<Betrieb>.Create(collectionBeforePageing, resourcePrameter.PageNumber, resourcePrameter.PageSize);
        }

        /// <summary>
        /// GetBetriebe
        /// </summary>
        /// <param name="id">Guid</param>
        /// <returns>IEnumerable<Betrieb></returns>
        public IEnumerable<Betrieb> GetBetriebe(IEnumerable<Guid> id)
        {
            return _context.Betriebe.Where(a => id.Contains(a.ID)).ToList();
        }

        /// <summary>
        /// UpdateBetrieb
        /// </summary>
        /// <param name="betrieb">Betrieb</param>
        public void UpdateBetrieb(Betrieb betrieb)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// BetriebExists
        /// </summary>
        /// <param name="id">Guidparam>
        /// <returns>bool</returns>
        public bool BetriebExists(Guid id)
        {
            return _context.Betriebe.Any(a => a.ID == id);
        }

        /// <summary>
        /// Save
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
        #endregion

        #region Mitarbeiter
        /// <summary>
        /// Mitarbeiter hinzufügen
        /// </summary>
        /// <param name="betriebId">BetriebID</param>
        /// <param name="mitarbeiter">Mitarbeiter</param>
        public void AddMitarbeiter(Guid betriebId, Mitarbeiter mitarbeiter)
        {
            var betrieb = GetBetrieb(betriebId);
            if (betrieb != null)
            {
                // if there isn't an id filled out (ie: we're not upserting),
                // we should generate one
                if (mitarbeiter.ID == Guid.Empty)
                {
                    mitarbeiter.ID = Guid.NewGuid();
                }
                betrieb.Mitarbeiters.Add(mitarbeiter);
            }
        }

        /// <summary>
        /// Mitarbeiter löschen
        /// </summary>
        /// <param name="mitarbeiter"></param>
        public void DeleteMitarbeiter(Mitarbeiter mitarbeiter)
        {
            _context.Mitarbeiters.Remove(mitarbeiter);
        }

        /// <summary>
        /// Einen bestimmten Mitarbeiter eines Betriebs
        /// </summary>
        /// <param name="betriebId">Betrieb ID</param>
        /// <param name="id"> Mitarbeiter ID</param>
        /// <returns> Mitarbeiter</returns>
        public Mitarbeiter GetMitarbeiter(Guid betriebId, Guid id)
        {
            return _context.Mitarbeiters
                        .Where(b => b.BetriebID == betriebId && b.ID == id).FirstOrDefault();
        }

        /// <summary>
        /// Mitarbeiter exisitiert
        /// </summary>
        /// <param name="betriebId"></param>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public bool MitarbeiterExists(Guid betriebId)
        {
            return _context.Mitarbeiters
                        .Any(b => b.BetriebID == betriebId && b.ID == betriebId);
        }

        /// <summary>
        /// Alle Mitarbeiter eines Betriebsladen
        /// </summary>
        /// <param name="betriebId">ID des Betriebs</param>
        /// <returns>IEnumerable von Mitarbeitern</returns>
        public IEnumerable<Mitarbeiter> GetMitarbeiters(Guid betriebId)
        {
            return _context.Mitarbeiters
                         .Where(b => b.BetriebID == betriebId).OrderBy(b => b.Nachname).ToList();
        }

        /// <summary>
        /// Update Mitarbeiter
        /// </summary>
        /// <param name="mitarbeiter"></param>
        public void UpdateMitarbeiter(Mitarbeiter mitarbeiter)
        {
            //throw new NotImplementedException();
        }
        #endregion

        #region BeregnungsDaten
        /// <summary>
        /// Hinzufügen neuer Daten
        /// </summary>
        /// <param name="daten">Neue Daten</param>
        public void AddBeregnungsDaten(Guid betriebId, BeregnungsDaten daten)
        {
            var betrieb = GetBetrieb(betriebId);
            if (betrieb != null)
            {
                // if there isn't an id filled out (ie: we're not upserting),
                // we should generate one
                if (daten.ID == Guid.Empty)
                {
                    daten.ID = Guid.NewGuid();
                }
                betrieb.BeregnungsDaten.Add(daten);
            }
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
        public BeregnungsDaten GetBeregnungsDaten(Guid betriebId, Guid id)
        {
            return _context.BeregnungsDatens
                        .Where(b => b.Betrieb.ID == betriebId && b.ID == id).FirstOrDefault();
        }

        /// <summary>
        /// GetDaten mit Seiten
        /// </summary>
        /// <param name="datenresourceParameters">Seiteneinstellungen</param>
        /// <returns> PagedList<Daten></returns>
        public PagedList<BeregnungsDaten> GetBeregnungsDatens(Guid betriebId, BeregnungsDatenResourceParameter datenresourceParameters)
        {
            //var collectionBeforPaging = _context.BeregnungsDatens.OrderBy(a =>
            //a.StartDatum).AsQueryable();
            var collectionBeforPaging =
                _context.BeregnungsDatens.ApplySort(datenresourceParameters.OrderBy,
                _propertyMappingService.GetPropertyMapping<BeregnungsDatenDto, BeregnungsDaten>());

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
        public IEnumerable<BeregnungsDaten> GetBeregnungsDatens(Guid betriebId)
        {
            return _context.BeregnungsDatens.Where(b => b.Betrieb.ID == betriebId).ToList();
        }

        public void UpdateBeregnungsDaten(BeregnungsDaten daten)
        {
            // throw new NotImplementedException();
        }
        #endregion

        /// <summary>
        /// AddSchlag
        /// </summary>
        /// <param name="schlag">Schlag</param>
        public void AddSchlag(Guid betriebId, Schlag schlag)
        {
            var betrieb = GetBetrieb(betriebId);
            if (betrieb != null)
            {
                if (schlag.ID == Guid.Empty)
                {
                    schlag.ID = Guid.NewGuid();
                    betrieb.Schlag.Add(schlag);
                }

            }
        }

        /// <summary>
        /// DeleteSchlag
        /// </summary>
        /// <param name="schlag">Schlag</param>
        public void DeleteSchlag(Schlag schlag)
        {
            _context.Schlaege.Remove(schlag);
        }

        /// <summary>
        /// GetSchlaege
        /// </summary>
        /// <param name="schlagRessource">SchlagResourceParameter</param>
        /// <returns>PagedList<Schlag></returns>
        public PagedList<Schlag> GetSchlaege(Guid betriebId, SchlagResourceParameter schlagRessource)
        {
            var collectionBeforPaging = _context.Schlaege.Where(b => b.Betrieb.ID == betriebId).OrderBy(a => a.Name);
            return PagedList<Schlag>.Create(collectionBeforPaging, schlagRessource.PageNumber, schlagRessource.PageSize);
        }

        /// <summary>
        /// GetSchlag
        /// </summary>
        /// <param name="Id">Guid</param>
        /// <returns>Schlag</returns>
        public Schlag GetSchlag(Guid betriebId, Guid Id)
        {
            return _context.Schlaege
                        .Where(b => b.Betrieb.ID == betriebId && b.ID == Id).FirstOrDefault();
        }

        /// <summary>
        /// GetSchlaege
        /// </summary>
        /// <param name="guids">Guid</param>
        /// <returns>IEnumerable<Schlag></returns>
        public IEnumerable<Schlag> GetSchlaege(Guid betriebId)
        {
            return _context.Schlaege.Where(b => b.Betrieb.ID == betriebId).ToList();
        }


        /// <summary>
        /// SchlagExists
        /// </summary>
        /// <param name="schlagId">Guid</param>
        /// <returns>bool</returns>
        public bool SchlagExists(Guid betriebID, Guid schlagId)
        {
            return _context.Schlaege.Any(a => a.Betrieb.ID == betriebID && a.ID == schlagId);
        }

        /// <summary>
        /// UpdateSchlag
        /// </summary>
        /// <param name="schlag">Schlag</param>
        public void UpdateSchlag(Schlag schlag)
        {
            //throw new NotImplementedException();
        }
    }
}
