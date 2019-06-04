using REST.Api.Entities;
using REST.Api.Helpers;
using System;
using System.Collections.Generic;


namespace REST.Api.Services
{
    public interface ISchlagRepository
    {
        PagedList<Schlag> GetSchlaege(ResourceParameters schlagResourceParameters);

        Schlag GetSchlaege(Guid guid);

        IEnumerable<Schlag> GetSchlaege(IEnumerable<Guid> guids);

        void AddSchlag(Schlag schlag);

        void DeleteSchlag(Schlag schlag);

        void UpdateSchlag(Schlag schlag);

        bool SchlagExists(Guid guid);

        bool Save();
    }
}
