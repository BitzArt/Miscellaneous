namespace BitzArt;

/// <summary>
/// A builder of a batch query.
/// </summary>
public interface IBatchQueryBuilder<TSource> : IQueryable<TSource>
{
    /// <summary>
    /// The batching strategy used by this query builder to compose the resulting query.
    /// </summary>
    public IBatchingStrategy<TSource> BatchingStrategy { get; set; }

    /// <summary>
    /// Notifies that the query has changed.
    /// </summary>
    public void NotifyQueryChanged();
}
