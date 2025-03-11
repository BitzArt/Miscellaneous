using System.Linq.Expressions;

namespace BitzArt.Linq.Conditional;

/// <summary>
/// Builder for a complex 'Where' condition.
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TMember"></typeparam>
public interface IWhereConditionBuilder<TSource, TMember> : IQueryable<TSource>
{
    internal IQueryable<TSource> Query { get; set; }
    internal Expression<Func<TSource, TMember>> Selector { get; set; }
}
