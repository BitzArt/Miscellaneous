using BitzArt;
using System.Linq.Expressions;

namespace System.Linq;

/// <summary>
/// Extensions for adding filters to LINQ queries.
/// </summary>
public static class AddFilterExtension
{
    /// <summary>
    /// Adds a filter to the query if the filter value is not null.
    /// </summary>
    public static IQueryable<TSource> AddFilter<TSource, TProperty>(this IQueryable<TSource> source, Expression<Func<TSource, TProperty?>> expression, TProperty? filter, FilterOperation filterOperation = FilterOperation.Equal)
        where TProperty : class
    {
        if (filter is null) return source;

        return BuildExpression(source, filter, expression, filterOperation);
    }

    /// <summary>
    /// Adds a filter to the query if the filter value is not null.
    /// </summary>
    public static IQueryable<TSource> AddFilter<TSource, TProperty>(this IQueryable<TSource> source, Expression<Func<TSource, TProperty?>> expression, TProperty? filter, FilterOperation filterOperation = FilterOperation.Equal)
        where TProperty : struct
    {
        if (filter is null) return source;

        var filterValue = filter!.Value;
        Expression<Func<TProperty?, TProperty>> getValueExpression = x => x!.Value;
        var valueExpression = expression.Compose(getValueExpression);

        return BuildExpression(source, filterValue, valueExpression, filterOperation);
    }

    private static IQueryable<TSource> BuildExpression<TSource, TProperty>(IQueryable<TSource> source, TProperty filter, Expression<Func<TSource, TProperty>> expression, FilterOperation filterOperation)
    {
        var argument = Expression.Parameter(typeof(TSource));
        var left = Expression.Invoke(expression, argument);
        var right = Expression.Constant(filter);

        var eq = filterOperation switch
        {
            FilterOperation.Equal => Expression.Equal(left, right),
            FilterOperation.NotEqual => Expression.NotEqual(left, right),
            FilterOperation.GreaterThan => Expression.GreaterThan(left, right),
            FilterOperation.GreaterThanOrEqual => Expression.GreaterThanOrEqual(left, right),
            FilterOperation.LessThan => Expression.LessThan(left, right),
            FilterOperation.LessThanOrEqual => Expression.LessThanOrEqual(left, right),
            _ => throw new NotImplementedException($"Unsupported Filter Operation: '{filterOperation}'")
        };

        var lambda = Expression
            .Lambda<Func<TSource, bool>>(eq, new[] { argument });

        return source.Where(lambda);
    }
}
