using System.Linq.Expressions;

namespace System.Linq;

/// <summary>
/// WhereIf extension for LINQ queries.
/// </summary>
public static class WhereIfExtension
{
    /// <summary>
    /// Filters the query if the condition is true.
    /// </summary>
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
    {
        return source.If(condition, x => x.Where(predicate));
    }
}
