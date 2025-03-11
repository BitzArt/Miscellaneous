using System.Collections;
using System.Linq.Expressions;

namespace BitzArt.Linq.Conditional;

internal class WhereConditionBuilder<TSource, TMember> : IWhereConditionBuilder<TSource, TMember>, IQueryable<TSource>
{
    public IQueryable<TSource> Query { get; set; }
    public Expression<Func<TSource, TMember>> Selector { get; set; }

    public Type ElementType => Query.ElementType;
    public Expression Expression => Query.Expression;
    public IQueryProvider Provider => Query.Provider;
    public IEnumerator<TSource> GetEnumerator() => Query.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Query).GetEnumerator();

    public WhereConditionBuilder(IQueryable<TSource> query, Expression<Func<TSource, TMember>> selector)
    {
        Query = query;
        Selector = selector;
    }
}
