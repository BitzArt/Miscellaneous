using System.Linq.Expressions;

namespace System.Linq;

/// <summary>
/// Batch extension for LINQ queries.
/// </summary>
public static class BatchExtensions
{
    /// <summary>
    /// Selects first batch, ordered by selector according to order direction.
    /// </summary>
    public static IQueryable<TSource> Batch<TSource, TProperty>(
        this IQueryable<TSource> query,
        Expression<Func<TSource, TProperty>> selector,
        int size,
        OrderDirection orderDirection = OrderDirection.Ascending)
    {
        return query.OrderBy(selector, orderDirection).Take(size);
    }

    /// <summary>
    /// Selects next batch with an offset, ordered by selector according to order direction.
    /// </summary>
    public static IQueryable<TSource> Batch<TSource, TProperty>(
        this IQueryable<TSource> query,
        Expression<Func<TSource, TProperty>> selector,
        (int size, int offset) batchRequest,
        OrderDirection orderDirection = OrderDirection.Ascending)
    {
        return query.OrderBy(selector, orderDirection)
            .Skip(batchRequest.offset)
            .Take(batchRequest.size);
    }

    /// <summary>
    /// Selects next batch, offset by last value, ordered by selector according to order direction.
    /// </summary>
    public static IQueryable<TSource> Batch<TSource, TProperty>(
        this IQueryable<TSource> query,
        Expression<Func<TSource, TProperty>> selector,
        int size,
        TProperty? lastValue,
        OrderDirection orderDirection = OrderDirection.Ascending)
    {
        if (lastValue is not null)
        {
            var constant = Expression.Constant(lastValue, typeof(TProperty));
            var comparison = orderDirection switch
            {
                OrderDirection.Ascending => Expression.GreaterThan(selector.Body, constant),
                OrderDirection.Descending => Expression.LessThan(selector.Body, constant),
                _ => throw new NotSupportedException($"Order direction '{orderDirection}' is not supported.")
            };

            var lambda = Expression.Lambda<Func<TSource, bool>>(comparison, selector.Parameters.First());
            query = query.Where(lambda);
        }

        return query.OrderBy(selector, orderDirection).Take(size);
    }
}
