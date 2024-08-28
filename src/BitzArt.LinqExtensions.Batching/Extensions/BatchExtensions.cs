using System.Linq.Expressions;

namespace BitzArt.LinqExtensions.Batching;

public static class BatchExtensions
{
    public static IBatchQueryBuilder<TSource> Batch<TSource>(this IQueryable<TSource> query, int size)
    {
        return new BatchQueryBuilder<TSource>(query, size);
    }

    public static IBatchQueryBuilder<TSource> ByOffset<TSource>(this IBatchQueryBuilder<TSource> builder, int offset)
    {
        builder.BatchingStrategy = new OffsetBatchingStrategy<TSource>(builder, offset);
        return builder;
    }

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
