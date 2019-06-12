using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using REST.Api.Entities;
using REST.Api.Helpers;

namespace REST.Api.Services
{
    public class BetriebRepository : IBetriebRepsoitory
    {
        private BeregnungsContext _context;

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
    }
}
