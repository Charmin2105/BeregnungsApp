using Beregnungs.App.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beregnungs.App.Services
{
    public interface IDataStore<T>
    {
        Task<bool> AddDatenAsync(T daten);
        Task<bool> UpdateDatenAsync(T daten);
        Task<bool> DeleteDatenAsync(Guid id);
        Task<T> GetDatenAsync(Guid id);
        Task<IEnumerable<T>> GetDatensAsync(bool forceRefresh = false);


    }
}
