using System.Linq;

namespace BitzArt;

/// <summary>
/// A set of filters to apply to a query.
/// </summary>
/// <typeparam name="TSource"></typeparam>
public interface IFilterSet<TSource> where TSource : class
{
    /// <summary>
    /// Applies the filter set to the query.
    /// </summary>
    public IQueryable<TSource> Apply(IQueryable<TSource> query);
}
