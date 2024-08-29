using System.Collections;
using System.Linq.Expressions;

namespace BitzArt.LinqExtensions.Batching;

internal class BatchQueryBuilder<TSource> : IBatchQueryBuilder<TSource>, IAsyncEnumerable<TSource>
{
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public IBatchingStrategy<TSource> BatchingStrategy { get; set; }

    private readonly int _size;
    private readonly IQueryable<TSource> _query;
    private bool _queryChanged = true;

    public IQueryable<TSource> ResultingQuery
    {
        get
        {
            if (_queryChanged)
            {
                _resultingQuery = BatchingStrategy.GetQuery(_query, _size);
                _queryChanged = false;
            }

            return _resultingQuery!;
        }
    }

    private IQueryable<TSource>? _resultingQuery;

    public Type ElementType => ResultingQuery.ElementType;
    public Expression Expression => ResultingQuery.Expression;
    public IQueryProvider Provider => ResultingQuery.Provider;

    public BatchQueryBuilder(IQueryable<TSource> query, int size)
    {
        _query = query;
        _size = size;
        BatchingStrategy = new BatchingStrategy<TSource>(this);
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public void NotifyQueryChanged()
    {
        _queryChanged = true;
    }

    public IEnumerator<TSource> GetEnumerator() => ResultingQuery.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        if (ResultingQuery is IAsyncEnumerable<TSource> asyncEnumerable)
            return asyncEnumerable.GetAsyncEnumerator(cancellationToken);

        throw new NotSupportedException("The query does not support async enumeration.");
    }
}
