using REST.Api.Entities;
using REST.Api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Services
{
    public interface IBetriebRepsoitory
    {
        PagedList<Betrieb> GetBetriebe(BetriebResourceParameter resourcePrameter);
        Betrieb GetBetrieb(Guid id);
        IEnumerable<Betrieb> GetBetriebe(IEnumerable<Guid> id);
        void AddBetrieb(Betrieb betrieb);
        void DeleteBetrieb(Betrieb betrieb);
        void UpdateBetrieb(Betrieb betrieb);
        bool BetriebExists(Guid id);
        bool Save();
    }
}
