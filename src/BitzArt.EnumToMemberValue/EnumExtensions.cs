using System;
using System.Linq;
using System.Runtime.Serialization;

namespace BitzArt.EnumToMemberValue
{
    public static class EnumExtensions
    {
        public static string ToMemberValue<TEnum>(this TEnum enumValue) where TEnum : struct, Enum
        {
            var member = typeof(TEnum).GetMember(enumValue.ToString()).First();
            var attribute = member.GetCustomAttributes(false).OfType<EnumMemberAttribute>().Single();
            return attribute.Value;
        }
    }
}
