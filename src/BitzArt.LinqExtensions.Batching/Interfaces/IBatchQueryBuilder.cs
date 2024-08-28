namespace BitzArt.LinqExtensions.Batching;

public interface IBatchQueryBuilder<TSource> : IQueryable<TSource>
{
    public IBatchingStrategy<TSource> BatchingStrategy { get; set; }

    public void NotifyQueryChanged();
}
