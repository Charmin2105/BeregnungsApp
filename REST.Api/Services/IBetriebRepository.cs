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

        #region Mitarbeiter
        Mitarbeiter GetMitarbeiter(Guid betriebId, Guid mitarbeiterId);
        IEnumerable<Mitarbeiter> GetMitarbeiters(Guid betriebId);
        void AddMitarbeiter(Guid betriebId, Mitarbeiter mitarbeiter);
        void DeleteMitarbeiter(Mitarbeiter mitarbeiter);
        void UpdateMitarbeiter(Mitarbeiter mitarbeiter);
        bool MitarbeiterExists(Guid mitarbeiterId);
        #endregion
        #region BeregnungsDaten
        PagedList<BeregnungsDaten> GetBeregnungsDatens(Guid betriebId, BeregnungsDatenResourceParameter datenresourceParameters);
        BeregnungsDaten GetBeregnungsDaten(Guid betriebId, Guid id);
        IEnumerable<BeregnungsDaten> GetBeregnungsDatens(Guid betriebId);
        void AddBeregnungsDaten(Guid betriebId, BeregnungsDaten daten);
        void DeleteBeregnungsDaten(BeregnungsDaten daten);
        void UpdateBeregnungsDaten(BeregnungsDaten daten);
        bool BeregnungsDatenExists(Guid guid);
        bool BetriebExists(Guid betriebId);
        #endregion
        #region Schlaege








        #endregion
        bool Save();
    }
}
