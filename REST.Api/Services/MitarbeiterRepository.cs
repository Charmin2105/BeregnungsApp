using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using REST.Api.Entities;
using REST.Api.Helpers;

namespace REST.Api.Services
{
    public class MitarbeiterRepository 
    {
        private BeregnungsContext _context;

        public MitarbeiterRepository(BeregnungsContext context)
        {
            _context = context;
        }

        public void AddMitarbeiter(Mitarbeiter account)
        {
            account.ID = Guid.NewGuid();
            _context.Mitarbeiters.Add(account);
        }

        public void DeleteMitarbeiter(Mitarbeiter account)
        {
            _context.Mitarbeiters.Remove(account);
        }
        public Mitarbeiter GetMitarbeiter(Guid id)
        {
            return _context.Mitarbeiters.FirstOrDefault(a => a.ID == id);
        }

        public bool MitarbeiterExists(Guid id)
        {
            return _context.Mitarbeiters.Any(a => a.ID == id);
        }

        public PagedList<Mitarbeiter> GetMitarbeiters(MitarbeiterResourceParameter resourceParameter)
        {
            var collectionBeforePageing = _context.Mitarbeiters.OrderBy(a => a.Nachname);
            return PagedList<Mitarbeiter>.Create(collectionBeforePageing, resourceParameter.PageNumber, resourceParameter.PageSize);
        }

        public IEnumerable<Mitarbeiter> GetMitarbeiters(IEnumerable<Guid> id)
        {
            return _context.Mitarbeiters.Where(a => id.Contains(a.ID)).ToList();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateMitarbeiter(Mitarbeiter account)
        {
            //throw new NotImplementedException();
        }
    }
}
