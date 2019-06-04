using REST.Api.Entities;
using REST.Api.Helpers;
using System;
using System.Collections.Generic;

namespace REST.Api.Services
{
    public interface IBeregnungsRepository
    {
        PagedList<BeregnungsDaten> GetBeregnungsDatens(ResourceParameters datenresourceParameters);
        BeregnungsDaten GetBeregnungsDaten(Guid id);
        IEnumerable<BeregnungsDaten> GetBeregnungsDatens(IEnumerable<Guid> guids);
        void AddBeregnungsDaten(BeregnungsDaten daten);
        void DeleteBeregnungsDaten(BeregnungsDaten daten);
        void UpdateBeregnungsDaten(BeregnungsDaten daten);
        bool BeregnungsDatenExists(Guid guid);
        bool Save();

    }
}