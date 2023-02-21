using BitzArt;
using System.Linq.Expressions;

namespace System.Linq;

public static class WhereIfExtension
{
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
    {
        if (condition) return source.Where(predicate);
        return source;
    }
}
