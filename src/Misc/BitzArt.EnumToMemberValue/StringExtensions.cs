using System;
using System.Linq;
using System.Runtime.Serialization;

namespace BitzArt.EnumToMemberValue;

/// <summary>
/// Extension methods for string-to-enum conversion.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Converts a string to an enum value.
    /// </summary>
    /// <typeparam name="TEnum">Enum type.</typeparam>
    /// <param name="enumString">The value to convert</param>
    /// <param name="defaultValue">Default value to return if the conversion fails.</param>
    /// <returns><typeparamref name="TEnum"/> value if the conversion is successful, otherwise <paramref name="defaultValue"/>. <br/>
    /// In case the conversion fails and <paramref name="defaultValue"/> is not provided, an <see cref="ArgumentException"/> is thrown.</returns>
    /// <exception cref="ArgumentException"></exception>
    public static TEnum ToEnum<TEnum>(this string enumString, TEnum? defaultValue = null)
        where TEnum : struct, Enum
    {
        var enumType = typeof(TEnum);
        foreach (var name in Enum.GetNames(enumType))
        {
            var attributes = enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true);
            if (attributes.Length == 0) continue;

            var enumMemberAttribute = (attributes as EnumMemberAttribute[]).Single();
            if (enumMemberAttribute.Value.Equals(enumString, StringComparison.CurrentCultureIgnoreCase))
                return (TEnum)Enum.Parse(enumType, name);
        }
        if (defaultValue.HasValue) return defaultValue.Value;
        throw new ArgumentException($"Can not parse '{enumString}' to enum '{typeof(TEnum).Name}'");
    }
}
