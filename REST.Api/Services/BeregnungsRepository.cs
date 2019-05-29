using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using REST.Api.Entities;

namespace REST.Api.Services
{
    public class BeregnungsRepository : IBeregnungsRepository
    {
        private BeregnungsContext _context;

        public BeregnungsRepository(BeregnungsContext context)
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

        public IEnumerable<Schlag> GetSchlaege()
        {
            return _context.Schlaege.ToList();
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
            throw new NotImplementedException();
        }
    }
}
