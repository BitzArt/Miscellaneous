namespace BitzArt;

/// <summary>
/// A strategy to compose a batch query.
/// </summary>
public interface IBatchingStrategy<TSource>
{
    /// <summary>
    /// Return a batch query composed by this strategy.
    /// </summary>
    public IQueryable<TSource> GetQuery(IQueryable<TSource> query, int size);

    /// <summary>
    /// Notifies that the query has changed.
    /// </summary>
    public void NotifyQueryChanged();
}
