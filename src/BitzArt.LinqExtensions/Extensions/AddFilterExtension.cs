using BitzArt;
using System.Linq.Expressions;

namespace System.Linq;

public static class AddFilterExtension
{
    public static IQueryable<TSource> AddFilter<TSource, TProperty>(this IQueryable<TSource> source, Expression<Func<TSource, TProperty?>> expression, TProperty? filter)
        where TProperty : class
    {
        if (filter is null) return source;

        return BuildExpression(source, filter, expression);
    }

    public static IQueryable<TSource> AddFilter<TSource, TProperty>(this IQueryable<TSource> source, Expression<Func<TSource, TProperty?>> expression, TProperty? filter)
        where TProperty : struct
    {
        if (filter is null) return source;

        var filterValue = filter!.Value;
        Expression<Func<TProperty?, TProperty>> getValueExpression = x => x!.Value;
        var valueExpression = expression.Compose(getValueExpression);

        return BuildExpression(source, filterValue, valueExpression);
    }

    private static IQueryable<TSource> BuildExpression<TSource, TProperty>(IQueryable<TSource> source, TProperty filter, Expression<Func<TSource, TProperty>> expression)
    {
        var argument = Expression.Parameter(typeof(TSource));
        var left = Expression.Invoke(expression, argument);
        var right = Expression.Constant(filter);
        var eq = Expression.Equal(left, right);

        var lambda = Expression
            .Lambda<Func<TSource, bool>>(eq, new[] { argument });

        return source.Where(lambda);
    }
}
