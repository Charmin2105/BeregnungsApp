using REST.Api.Entities;
using REST.Api.Helpers;
using System;
using System.Collections.Generic;

namespace REST.Api.Services
{
    /// <summary>
    /// interface IBeregnungsRepository
    /// </summary>
    public interface IBeregnungsRepository
    {
        PagedList<BeregnungsDaten> GetBeregnungsDatens(BeregnungsDatenResourceParameter datenresourceParameters);
        BeregnungsDaten GetBeregnungsDaten(Guid id);
        IEnumerable<BeregnungsDaten> GetBeregnungsDatens(IEnumerable<Guid> guids);
        void AddBeregnungsDaten(BeregnungsDaten daten);
        void DeleteBeregnungsDaten(BeregnungsDaten daten);
        void UpdateBeregnungsDaten(BeregnungsDaten daten);
        bool BeregnungsDatenExists(Guid guid);
        bool Save();

    }
}