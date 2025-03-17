namespace BitzArt;

internal static class TypeExtensions
{
    public static bool IsNullable(this Type type)
        => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
}
