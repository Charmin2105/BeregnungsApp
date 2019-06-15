using REST.Api.Entities;
using REST.Api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Services
{
    /// <summary>
    /// interface IBetriebRepository
    /// </summary>
    public interface IBetriebRepository
    {
        PagedList<Betrieb> GetBetriebe(BetriebResourceParameter resourcePrameter);
        Betrieb GetBetrieb(Guid betriebId);
        IEnumerable<Betrieb> GetBetriebe(IEnumerable<Guid> betriebId);
        void AddBetrieb(Betrieb betrieb);
        void DeleteBetrieb(Betrieb betrieb);
        void UpdateBetrieb(Betrieb betrieb);
        Mitarbeiter GetMitarbeiter(Guid betriebId,Guid mitarbeiterId);
        IEnumerable<Mitarbeiter> GetMitarbeiters(Guid betriebId);
        void AddMitarbeiter(Guid betriebId,Mitarbeiter mitarbeiter);
        void DeleteMitarbeiter(Mitarbeiter mitarbeiter);
        void UpdateMitarbeiter(Mitarbeiter mitarbeiter);
        bool MitarbeiterExists(Guid mitarbeiterId);
        bool BetriebExists(Guid betriebId);
        bool Save();
    }
}
