namespace System.Linq;

/// <summary>
/// If extension for LINQ queries.
/// </summary>
public static class IfExtension
{
    /// <summary>
    /// Executes the transform function on the source if the condition is <see langword="true"/>
    /// </summary>
    public static IQueryable<T> If<T>(this IQueryable<T> source, bool condition, Func<IQueryable<T>, IQueryable<T>> transform)
    {
        if (condition) return transform.Invoke(source);
        return source;
    }
}
