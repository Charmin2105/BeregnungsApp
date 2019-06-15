using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using REST.Api.Entities;
using REST.Api.Helpers;

namespace REST.Api.Services
{
    /// <summary>
    /// class MitarbeiterRepository
    /// </summary>
    public class MitarbeiterRepository 
    {
        private BeregnungsContext _context;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">BeregnungsContext</param>
        public MitarbeiterRepository(BeregnungsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// AddMitarbeiter
        /// </summary>
        /// <param name="mitarbeiter">Mitarbeiter</param>
        public void AddMitarbeiter(Mitarbeiter mitarbeiter)
        {
            mitarbeiter.ID = Guid.NewGuid();
            _context.Mitarbeiters.Add(mitarbeiter);
        }

        /// <summary>
        /// DeleteMitarbeiter
        /// </summary>
        /// <param name="mitarbeiter">Mitarbeiter</param>
        public void DeleteMitarbeiter(Mitarbeiter mitarbeiter)
        {
            _context.Mitarbeiters.Remove(mitarbeiter);
        }

        /// <summary>
        /// GetMitarbeiter
        /// </summary>
        /// <param name="id">Guid</param>
        /// <returns></returns>
        public Mitarbeiter GetMitarbeiter(Guid id)
        {
            return _context.Mitarbeiters.FirstOrDefault(a => a.ID == id);
        }

        /// <summary>
        /// MitarbeiterExists
        /// </summary>
        /// <param name="id">Guid</param>
        /// <returns>bool</returns>
        public bool MitarbeiterExists(Guid id)
        {
            return _context.Mitarbeiters.Any(a => a.ID == id);
        }

        /// <summary>
        /// GetMitarbeiters
        /// </summary>
        /// <param name="resourceParameter">MitarbeiterResourceParameter</param>
        /// <returns>PagedList<Mitarbeiter></returns>
        public PagedList<Mitarbeiter> GetMitarbeiters(MitarbeiterResourceParameter resourceParameter)
        {
            var collectionBeforePageing = _context.Mitarbeiters.OrderBy(a => a.Nachname);
            return PagedList<Mitarbeiter>.Create(collectionBeforePageing, resourceParameter.PageNumber, resourceParameter.PageSize);
        }

        /// <summary>
        /// GetMitarbeiters
        /// </summary>
        /// <param name="id">Guid</param>
        /// <returns>IEnumerable<Mitarbeiter></returns>
        public IEnumerable<Mitarbeiter> GetMitarbeiters(IEnumerable<Guid> id)
        {
            return _context.Mitarbeiters.Where(a => id.Contains(a.ID)).ToList();
        }

        /// <summary>
        /// Save
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        /// <summary>
        /// UpdateMitarbeiter
        /// </summary>
        /// <param name="mitarbeiter">Mitarbeiter</param>
        public void UpdateMitarbeiter(Mitarbeiter mitarbeiter)
        {
            //throw new NotImplementedException();
        }
    }
}
