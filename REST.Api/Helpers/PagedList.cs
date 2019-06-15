using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Helpers
{
    /// <summary>
    /// PagedList
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        /// <summary>
        /// Abfrage ob es eine vorherige Seite gibt
        /// </summary>
        public bool HasPrevious
        {
            get
            {
                return (CurrentPage > 1);
            }
        }

        /// <summary>
        /// Abfrage ob es eine nachfolger Seite gibt
        /// </summary>
        public bool HasNext
        {
            get
            {
                return (CurrentPage < TotalPages);
            }
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="items">ListItems</param>
        /// <param name="count">Seiten Anzahl</param>
        /// <param name="pageNumber">Seitenzahl</param>
        /// <param name="pageSize">Inhalte Pro Seite</param>
        public PagedList(List<T> items,int count,int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        /// <summary>
        /// Create PageList
        /// </summary>
        /// <param name="source">IQueryable</param>
        /// <param name="pageNumber">Seitenzahl</param>
        /// <param name="pageSize">Inhalt Pro Seite</param>
        /// <returns></returns>
        public static PagedList<T> Create(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

    }
}
