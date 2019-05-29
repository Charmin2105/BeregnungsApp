using BeregnungsRESTapi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeregnungsRESTapi.Services
{
    public interface IBeregnungsdatenRepository
    {
        IEnumerable<Beregnungs> GetBeregnungs();

        Beregnungs GetBeregnung(Guid Id);

        IEnumerable<Beregnungs> GetBeregnungs(IEnumerable<Guid> BergegnungsId);

        void AddBergenungsdaten(Beregnungs beregnungsdaten);

        void DeleteBergenungsdaten(Beregnungs beregnungsdaten);

        void UpdateBergenungsdaten(Beregnungs beregnungsdaten);

        void BergenungsdatenExists(Beregnungs beregnungsdaten);

        bool Save();
    }
}
