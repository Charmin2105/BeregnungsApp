using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using REST.Api.Entities;
using REST.Api.Helpers;

namespace REST.Api.Services
{
    /// <summary>
    /// SchlagRepository
    /// </summary>
    public class SchlagRepository : ISchlagRepository
    {
        private BeregnungsContext _context;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context"></param>
        public SchlagRepository(BeregnungsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// AddSchlag
        /// </summary>
        /// <param name="schlag">Schlag</param>
        public void AddSchlag(Schlag schlag)
        {
            schlag.ID = Guid.NewGuid();
            _context.Schlaege.Add(schlag);
        }

        /// <summary>
        /// DeleteSchlag
        /// </summary>
        /// <param name="schlag">Schlag</param>
        public void DeleteSchlag(Schlag schlag)
        {
            _context.Schlaege.Remove(schlag);
        }

        /// <summary>
        /// GetSchlaege
        /// </summary>
        /// <param name="schlagRessource">SchlagResourceParameter</param>
        /// <returns>PagedList<Schlag></returns>
        public PagedList<Schlag> GetSchlaege(SchlagResourceParameter schlagRessource)
        {
            var collectionBeforPaging = _context.Schlaege.OrderBy(a => a.Name);
            return PagedList<Schlag>.Create(collectionBeforPaging, schlagRessource.PageNumber, schlagRessource.PageSize);
        }

        /// <summary>
        /// GetSchlag
        /// </summary>
        /// <param name="Id">Guid</param>
        /// <returns>Schlag</returns>
        public Schlag GetSchlag(Guid Id)
        {
            return _context.Schlaege.FirstOrDefault(a => a.ID == Id);
        }

        /// <summary>
        /// GetSchlaege
        /// </summary>
        /// <param name="guids">Guid</param>
        /// <returns>IEnumerable<Schlag></returns>
        public IEnumerable<Schlag> GetSchlaege(IEnumerable<Guid> guids)
        {
            return _context.Schlaege.Where(a => guids.Contains(a.ID)).ToList();
        }

        /// <summary>
        /// Save
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        /// <summary>
        /// SchlagExists
        /// </summary>
        /// <param name="schlagId">Guid</param>
        /// <returns>bool</returns>
        public bool SchlagExists(Guid schlagId)
        {
            return _context.Schlaege.Any(a => a.ID == schlagId);
        }

        /// <summary>
        /// UpdateSchlag
        /// </summary>
        /// <param name="schlag">Schlag</param>
        public void UpdateSchlag(Schlag schlag)
        {
            //throw new NotImplementedException();
        }
    }
}
