using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace REST.Api.Helpers
{
    public static class IEnumerableExtensions
    {

        public static IEnumerable<ExpandoObject> ShapeData<TSource>(this IEnumerable<TSource> source, string fields)
        {
            if (source == null)
            {
                throw new ArgumentException("source");
            }

            //Liste mit ExpandoObjects erstellen
            var expandoObjectList = new List<ExpandoObject>();

            //Liste mit PropertyInfo Objekten von TSource erstellen
            var propertyInfoList = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                // alle public properties sollten in ExpandoObject enthalten sein
                var propertyInfo = typeof(TSource)
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance);
                propertyInfoList.AddRange(propertyInfo);
            }
            else
            {
                // Nur public properties die mit "fields" übereinstimmen sollten in ExpandoObject sein

                //fields sind durch "," getrent
                var fieldsAfterSplit = fields.Split(',');

                foreach (var field in fieldsAfterSplit)
                {
                    var propertyName = field.Trim();

                    var propertyInfo = typeof(TSource).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo == null)
                    {
                        throw new Exception($"Property {propertyName} nicht gefunden bei {typeof(TSource)}.");
                    }
                    // Hinzugfügen von propertyInfo zu List
                    propertyInfoList.Add(propertyInfo);
                }
            }
            //gehe durch Source Objekte
            foreach (var sourceObject in source)
            {
                var dataShapedObject = new ExpandoObject();
                foreach (var propertyInfo in propertyInfoList)
                {
                    var propertyValue = propertyInfo.GetValue(sourceObject);

                    ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, propertyValue);
                }
                expandoObjectList.Add(dataShapedObject);
            }

            return expandoObjectList;
        }
    }
}
