using System;
using System.Linq;
using System.Runtime.Serialization;

namespace BitzArt.EnumToMemberValue
{
    public static class StringExtensions
    {
        public static TEnum ToEnum<TEnum>(this string enumString, TEnum? defaultValue = null)
            where TEnum : struct, Enum
        {
            var enumType = typeof(TEnum);
            foreach (var name in Enum.GetNames(enumType))
            {
                var attributes = enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true);
                if (!attributes.Any()) continue;
                var enumMemberAttribute = (attributes as EnumMemberAttribute[]).Single();
                if (enumMemberAttribute.Value.ToLower() == enumString.ToLower()) return (TEnum)Enum.Parse(enumType, name);
            }
            if (defaultValue.HasValue) return defaultValue.Value;
            throw new ArgumentException($"Can not parse '{enumString}' to enum '{typeof(TEnum).Name}'");
        }
    }
}
