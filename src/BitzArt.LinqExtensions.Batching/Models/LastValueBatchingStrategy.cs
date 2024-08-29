using System.Linq.Expressions;

namespace BitzArt.LinqExtensions.Batching;

internal class LastValueBatchingStrategy<TSource, TProperty>(
    IBatchQueryBuilder<TSource> builder,
    Expression<Func<TSource, TProperty>> selector,
    TProperty? lastValue,
    OrderDirection orderDirection)
    : BatchingStrategy<TSource>(builder)
{
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public override IQueryable<TSource> GetQuery(IQueryable<TSource> query, int size)
    {
        if (lastValue is null)
            return query.OrderBy(selector, orderDirection).Take(size);

        var constant = Expression.Constant(lastValue, typeof(TProperty));
        var comparison = orderDirection switch
        {
            OrderDirection.Ascending => Expression.GreaterThan(selector.Body, constant),
            OrderDirection.Descending => Expression.LessThan(selector.Body, constant),
            _ => throw new NotSupportedException($"Order direction '{orderDirection}' is not supported.")
        };

        var lambda = Expression.Lambda<Func<TSource, bool>>(comparison, selector.Parameters.First());

        return query.OrderBy(selector, orderDirection)
            .Where(lambda)
            .Take(size);
    }
}
