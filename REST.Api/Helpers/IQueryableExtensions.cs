using REST.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace REST.Api.Helpers
{
    /// <summary>
    /// IQueryableExtensions
    /// </summary>
    public static class IQueryableExtensions
    {
        /// <summary>
        /// ApplySort
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderBy"></param>
        /// <param name="mappingDictionary"></param>
        /// <returns></returns>
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source,
            string orderBy, Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            //Input Check
            if (source == null)
            {
                throw new ArgumentException("source");
            }
            if (mappingDictionary == null)
            {
                throw new ArgumentException("source");
            }
            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }

            // Der orderBy string anpassen
            var orderByAfterSplit = orderBy.Split(',');

            //Anwenden der orderBy Clause in Rückwärtiger Order
            // IQueryable sortiert sonst in falscher Order
            foreach (var orderByClause in orderByAfterSplit.Reverse())
            {
                //Trim
                var trimmedOrderByClause = orderByClause.Trim();

                //Wenn Sortieroption endet mit Desc wird in absteigender Reihenfolge Sortiert
                var orderDescending = trimmedOrderByClause.EndsWith(" desc");

                //Entfernen von "asc" oder "desc" aus orderByClause
                var indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedOrderByClause : trimmedOrderByClause.Remove(indexOfFirstSpace);

                //finde passende Einträge
                if (!mappingDictionary.ContainsKey(propertyName))
                {
                    throw new ArgumentException($" Schlüssel Mapping für {propertyName} fehlt");
                }

                //get PropertyMappingValue
                var propertyMappingValue = mappingDictionary[propertyName];

                if (propertyMappingValue == null)
                {
                    throw new ArgumentNullException("propertyMappingValue");
                }

                foreach (var destinationProperty in propertyMappingValue.DestinationPropertise.Reverse())
                {
                    if (propertyMappingValue.Revert)
                    {
                        orderDescending = !orderDescending;
                    }
                    source = source.OrderBy(destinationProperty + (orderDescending ? " descending" : " ascending"));
                }
            }
            return source;
        }
    }
}
