using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using REST.Api.Entities;
using REST.Api.Helpers;

namespace REST.Api.Services
{
    public class BetriebRepository : IBetriebRepository
    {
        private BeregnungsContext _context;

        #region Betrieb
        public BetriebRepository(BeregnungsContext context)
        {
            _context = context;
        }

        public void AddBetrieb(Betrieb betrieb)
        {
            betrieb.ID = Guid.NewGuid();
            _context.Betriebe.Add(betrieb);
        }

        public void DeleteBetrieb(Betrieb betrieb)
        {
            _context.Betriebe.Remove(betrieb);
        }

        public Betrieb GetBetrieb(Guid id)
        {
            return _context.Betriebe.FirstOrDefault(a => a.ID == id);
        }

        public PagedList<Betrieb> GetBetriebe(BetriebResourceParameter resourcePrameter)
        {
            var collectionBeforePageing = _context.Betriebe.OrderBy(a => a.Name);
            return PagedList<Betrieb>.Create(collectionBeforePageing, resourcePrameter.PageNumber, resourcePrameter.PageSize);
        }

        public IEnumerable<Betrieb> GetBetriebe(IEnumerable<Guid> id)
        {
            return _context.Betriebe.Where(a => id.Contains(a.ID)).ToList();
        }

        public void UpdateBetrieb(Betrieb betrieb)
        {
            //throw new NotImplementedException();
        }

        public bool BetriebExists(Guid id)
        {
            return _context.Betriebe.Any(a => a.ID == id);
        }

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
        public void UpdateMitarbeiter( Mitarbeiter mitarbeiter)
        {
            //throw new NotImplementedException();
        } 
        #endregion
    }
}
