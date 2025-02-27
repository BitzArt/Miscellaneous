using System;
using System.Linq;
using System.Runtime.Serialization;

namespace BitzArt.EnumToMemberValue;

/// <summary>
/// Extension methods for enum-to-string conversion.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Resolves the EnumMemberAttribute value of the enum value.
    /// </summary>
    /// <typeparam name="TEnum">Enum type</typeparam>
    /// <param name="enumValue">The value to resolve EnumMemberAttribute value for.</param>
    /// <returns>Value of the EnumMemberAttribute</returns>
    public static string ToMemberValue<TEnum>(this TEnum enumValue) where TEnum : struct, Enum
    {
        var member = typeof(TEnum).GetMember(enumValue.ToString()).First();
        var attribute = member.GetCustomAttributes(false).OfType<EnumMemberAttribute>().Single();
        return attribute.Value;
    }
}
