namespace BitzArt;

internal static class TypeExtensions
{
    public static bool IsNullable(this Type type, out Type? underlyingType)
    {
        underlyingType = null;

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) == false)
            return false;

        underlyingType = Nullable.GetUnderlyingType(type);

        return true;
    }
}
