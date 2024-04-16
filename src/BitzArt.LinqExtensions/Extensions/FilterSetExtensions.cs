using BitzArt;

namespace System.Linq;

/// <summary>
/// Extension methods for applying filter sets to queries.
/// </summary>
public static class FilterSetExtensions
{
    /// <summary>
    /// Applies the filter set to the query.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="query"></param>
    /// <param name="filterSet"></param>
    /// <returns></returns>
    public static IQueryable<TSource> Apply<TSource>(this IQueryable<TSource> query, IFilterSet<TSource> filterSet)
        where TSource : class
    {
        return filterSet.Apply(query);
    }
}
