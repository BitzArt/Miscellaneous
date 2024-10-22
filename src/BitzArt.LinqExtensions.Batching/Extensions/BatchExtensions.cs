using BitzArt;
using BitzArt.LinqExtensions.Batching;
using System.Linq.Expressions;

namespace System.Linq;

/// <summary>
/// Batching extensions for LINQ queries.
/// </summary>
public static class BatchExtensions
{
    /// <summary>
    /// Selects batch of specified size.
    /// </summary>
    public static IBatchQueryBuilder<TSource> Batch<TSource>(this IQueryable<TSource> query, int size)
        => new BatchQueryBuilder<TSource>(query, size);

    /// <summary>
    /// Applies offset to batch.
    /// </summary>
    public static IBatchQueryBuilder<TSource> ByOffset<TSource>(this IBatchQueryBuilder<TSource> builder, int offset)
    {
        builder.BatchingStrategy = new OffsetBatchingStrategy<TSource>(builder, offset);
        return builder;
    }

    /// <summary>
    /// Applies offset by last value to batch, ordered by selector according to order direction.
    /// </summary>
    public static IBatchQueryBuilder<TSource> ByLastValue<TSource, TProperty>(
        this IBatchQueryBuilder<TSource> builder,
        Expression<Func<TSource, TProperty>> selector,
        TProperty? lastValue,
        OrderDirection orderDirection = OrderDirection.Ascending)
    {
        builder.BatchingStrategy = new LastValueBatchingStrategy<TSource, TProperty>(builder, selector, lastValue, orderDirection);
        return builder;
    }
}
