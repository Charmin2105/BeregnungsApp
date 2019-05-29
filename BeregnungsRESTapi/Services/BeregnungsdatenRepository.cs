using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeregnungsRESTapi.Entities;

namespace BeregnungsRESTapi.Services
{
    public class BeregnungsdatenRepository : IBeregnungsdatenRepository
    {
        private BeregnungsContext _context;

        public BeregnungsdatenRepository(BeregnungsContext context)
        {
            _context = context;
        }
        public void AddBergenungsdaten(Beregnungs beregnungsdaten)
        {
            throw new NotImplementedException();
        }

        public void BergenungsdatenExists(Beregnungs beregnungsdaten)
        {
            throw new NotImplementedException();
        }

        public void DeleteBergenungsdaten(Beregnungs beregnungsdaten)
        {
            throw new NotImplementedException();
        }

        public Beregnungs GetBeregnung(Guid Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Beregnungs> GetBeregnungs()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Beregnungs> GetBeregnungs(IEnumerable<Guid> BergegnungsId)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public void UpdateBergenungsdaten(Beregnungs beregnungsdaten)
        {
            throw new NotImplementedException();
        }
    }
}
