namespace BitzArt.LinqExtensions.Batching;

public interface IBatchingStrategy<TSource>
{
    public IQueryable<TSource> GetQuery(IQueryable<TSource> query, int size);
    public void NotifyQueryChanged();
}
