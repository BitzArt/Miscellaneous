using System.Linq.Expressions;

namespace System.Linq;

/// <summary>
/// LINQ extensions for ordering queries.
/// </summary>
public static class OrderExtensions
{
    /// <summary>
    /// Orders the query by the specified key selector and direction.
    /// </summary>
    public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> query, Expression<Func<TSource, TKey>> keySelector, OrderDirection direction)
    {
        if (direction == OrderDirection.Ascending)
        {
            return query.OrderBy(keySelector);
        }

        if (direction == OrderDirection.Descending)
        {
            return query.OrderByDescending(keySelector);
        }

        throw new InvalidOperationException("Invalid order direction.");
    }

    /// <summary>
    /// Orders the query by the specified key selector and direction.
    /// </summary>
    public static IOrderedQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedQueryable<TSource> query, Expression<Func<TSource, TKey>> keySelector, OrderDirection direction)
    {
        if (direction == OrderDirection.Ascending)
        {
            return query.ThenBy(keySelector);
        }

        if (direction == OrderDirection.Descending)
        {
            return query.ThenByDescending(keySelector);
        }

        throw new InvalidOperationException("Invalid order direction.");
    }
}
