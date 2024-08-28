
namespace BitzArt.LinqExtensions.Batching;

internal class BatchingStrategy<TSource> : IBatchingStrategy<TSource>
{
    protected readonly IBatchQueryBuilder<TSource> Builder;

    public BatchingStrategy(IBatchQueryBuilder<TSource> builder)
    {
        Builder = builder;
        NotifyQueryChanged();
    }

    public virtual IQueryable<TSource> GetQuery(IQueryable<TSource> query, int size)
    {
        return query.Take(size);
    }

    public void NotifyQueryChanged()
    {
        Builder.NotifyQueryChanged();
    }
}
