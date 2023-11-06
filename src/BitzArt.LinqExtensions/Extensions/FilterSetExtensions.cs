using System.Linq;

namespace BitzArt;

public static class FilterSetExtensions
{
    public static IQueryable<TSource> Apply<TSource>(this IQueryable<TSource> query, IFilterSet<TSource> filterSet)
        where TSource : class
    {
        return filterSet.Apply(query);
    }
}
