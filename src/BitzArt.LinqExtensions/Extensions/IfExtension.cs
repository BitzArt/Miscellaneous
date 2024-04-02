using System.Linq.Expressions;

namespace System.Linq;

public static class IfExtension
{
    public static IQueryable<T> If<T>(this IQueryable<T> source, bool condition, Expression<Func<IQueryable<T>, IQueryable<T>>> expression)
    {
        if (condition) return expression.Compile().Invoke(source);
        return source;
    }
}
