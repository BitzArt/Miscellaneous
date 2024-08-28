namespace BitzArt.LinqExtensions.Batching;

internal class OffsetBatchingStrategy<TSource>(IBatchQueryBuilder<TSource> builder, int offset) 
    : BatchingStrategy<TSource>(builder)
{
    private readonly int _offset = offset;

    public override IQueryable<TSource> GetQuery(IQueryable<TSource> query, int size)
    {
        return query.Skip(_offset).Take(size);
    }
}
