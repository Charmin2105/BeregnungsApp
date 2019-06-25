using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beregnungs.App.Services
{
    public interface IDataStore<T>
    {
        Task<bool> AddAsync(T item);
        Task<bool> UpdateAsync(T item);
        Task<bool> DeleteAsync(Guid id);
        Task<T> GetAsync(Guid id);
        Task<IEnumerable<T>> GetsAsync(bool forceRefresh = false);
    }
}
