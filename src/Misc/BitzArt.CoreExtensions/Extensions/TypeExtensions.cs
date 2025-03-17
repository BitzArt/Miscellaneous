namespace BitzArt;

/// <summary>
/// Provides a set of extension methods for <see cref="Type"/>.
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// Determines whether the specified type is nullable.
    /// </summary>
    public static bool IsNullable(this Type type, out Type? underlyingType)
    {
        underlyingType = null;

        if ((type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) == false)
            return false;

        underlyingType = Nullable.GetUnderlyingType(type);

        return true;
    }
}
