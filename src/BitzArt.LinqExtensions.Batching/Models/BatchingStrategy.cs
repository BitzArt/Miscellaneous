namespace BitzArt;

internal class BatchingStrategy<TSource> : IBatchingStrategy<TSource>
{
    protected readonly IBatchQueryBuilder<TSource> Builder;

    public BatchingStrategy(IBatchQueryBuilder<TSource> builder)
    {
        Builder = builder;
        NotifyQueryChanged();
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public virtual IQueryable<TSource> GetQuery(IQueryable<TSource> query, int size)
        => query.Take(size);

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public void NotifyQueryChanged() 
        => Builder.NotifyQueryChanged();
}
