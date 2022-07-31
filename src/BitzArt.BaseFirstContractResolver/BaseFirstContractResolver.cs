using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BitzArt.BaseFirstContractResolver
{
    public class BaseFirstContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);
            if (properties != null)
                return properties.OrderBy(p => p.DeclaringType.BaseTypesAndSelf().Count()).ToList();

            return properties;
        }

        protected override string ResolvePropertyName(string propertyName)
        {
            return char.ToLowerInvariant(propertyName[0]) + propertyName.Remove(0, 1);
        }
    }

    public static class TypeExtensions
    {
        public static IEnumerable<Type> BaseTypesAndSelf(this Type type)
        {
            while (type != null)
            {
                yield return type;
                type = type.BaseType;
            }
        }
    }
}
