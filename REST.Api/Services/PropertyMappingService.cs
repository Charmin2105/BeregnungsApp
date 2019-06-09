using REST.Api.Entities;
using REST.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REST.Api.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _beregnungsPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>() {"Id" }) },
                {"StartDatum", new PropertyMappingValue(new List<string>() {"StartDatum" }) },
                {"StartUhrzeit", new PropertyMappingValue(new List<string>() {"StartUhrzeit" }) },
                {"EndDatum", new PropertyMappingValue(new List<string>() {"EndDatum" }) },
                {"Betrieb", new PropertyMappingValue(new List<string>() {"Betrieb" }) },
                {"SchlagID", new PropertyMappingValue(new List<string>() {"SchlagID" }) },
                {"Duese", new PropertyMappingValue(new List<string>() {"Duese" }) },
                {"WasseruhrAnfang", new PropertyMappingValue(new List<string>() {"WasseruhrAnfang" }) },
                {"WasseruhrEnde", new PropertyMappingValue(new List<string>() {"WasseruhrEnde" }) },
                {"Vorkomnisse", new PropertyMappingValue(new List<string>() {"Vorkomnisse" }) },
                {"IstAbgeschlossen", new PropertyMappingValue(new List<string>() {"IstAbgeschlossen" }) }
            };

        private IList<IPropertyMapping> propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            propertyMappings.Add(new PropertyMapping<BeregnungsDatenDto,
                BeregnungsDaten>(_beregnungsPropertyMapping));
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping
            <TSource, TDestionation>()
        {
            //Get matching Mappings
            var matchingMapping = propertyMappings.OfType<PropertyMapping<TSource, TDestionation>>();

            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First()._mappingDictionary;
            }

            throw new Exception($"Konnte kein passenden Property Mapping für <{(typeof(TSource))} finden.");
        }
        public bool ValidMappingExistsFor<TSource, TDestination>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }
            var fieldsAfterSplit = fields.Split(',');

            foreach (var field in fieldsAfterSplit)
            {
                var trimmedField = field.Trim();

                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedField : trimmedField.Remove(indexOfFirstSpace);

                if (!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
