using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using REST.Api.Entities;
using REST.Api.Helpers;

namespace REST.Api.Services
{
    public class SchlagRepository : ISchlagRepository
    {
        private BeregnungsContext _context;

        public SchlagRepository(BeregnungsContext context)
        {
            _context = context;
        }

        public void AddSchlag(Schlag schlag)
        {
            schlag.ID = Guid.NewGuid();
            _context.Schlaege.Add(schlag);
        }

        public void DeleteSchlag(Schlag schlag)
        {
            _context.Schlaege.Remove(schlag);
        }

        public PagedList<Schlag> GetSchlaege(ResourceParameters schlagRessource)
        {
            var collectionBeforPaging = _context.Schlaege.OrderBy(a => a.Name);
            return PagedList<Schlag>.Create(collectionBeforPaging, schlagRessource.PageNumber, schlagRessource.PageSize);
        }

        public Schlag GetSchlaege(Guid Id)
        {
            return _context.Schlaege.FirstOrDefault(a => a.ID == Id);
        }

        public IEnumerable<Schlag> GetSchlaege(IEnumerable<Guid> guids)
        {
            return _context.Schlaege.Where(a => guids.Contains(a.ID)).ToList();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public bool SchlagExists(Guid schlagId)
        {
            return _context.Schlaege.Any(a => a.ID == schlagId);
        }

        public void UpdateSchlag(Schlag schlag)
        {
            //throw new NotImplementedException();
        }
    }
}
