namespace BitzArt;

internal class OffsetBatchingStrategy<TSource>(IBatchQueryBuilder<TSource> builder, int offset)
    : BatchingStrategy<TSource>(builder)
{
    private readonly int _offset = offset;

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public override IQueryable<TSource> GetQuery(IQueryable<TSource> query, int size)
        => query.Skip(_offset).Take(size);
}
