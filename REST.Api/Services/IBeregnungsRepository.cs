using REST.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Services
{
    public interface IBeregnungsRepository
    {
        IEnumerable<Schlag> GetSchlaege();

        Schlag GetSchlaege(Guid Id);

        IEnumerable<Schlag> GetSchlaege(IEnumerable<Guid> guids);

        void AddSchlag(Schlag schlag);

        void DeleteSchlag(Schlag schlag);

        void UpdateSchlag(Schlag schlag);

        bool SchlagExists(Guid authorId);

        bool Save();
    }
}
