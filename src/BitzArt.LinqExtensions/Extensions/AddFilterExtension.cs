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
    public static IQueryable<TSource> AddFilter<TSource, TProperty>(this IQueryable<TSource> source, Expression<Func<TSource, TProperty?>> expression, TProperty? filter, ComparisonType comparisonType = ComparisonType.Equal)
        where TProperty : class
    {
        if (filter is null) return source;

        return BuildExpression(source, filter, expression, comparisonType);
    }

    /// <summary>
    /// Adds a filter to the query if the filter value is not null.
    /// </summary>
    public static IQueryable<TSource> AddFilter<TSource, TProperty>(this IQueryable<TSource> source, Expression<Func<TSource, TProperty?>> expression, TProperty? filter, ComparisonType comparisonType = ComparisonType.Equal)
        where TProperty : struct
    {
        if (filter is null) return source;

        var filterValue = filter!.Value;
        Expression<Func<TProperty?, TProperty>> getValueExpression = x => x!.Value;
        var valueExpression = expression.Compose(getValueExpression);

        return BuildExpression(source, filterValue, valueExpression, comparisonType);
    }

    private static IQueryable<TSource> BuildExpression<TSource, TProperty>(IQueryable<TSource> source, TProperty filter, Expression<Func<TSource, TProperty>> expression, ComparisonType comparisonType)
    {
        var argument = Expression.Parameter(typeof(TSource));
        var left = Expression.Invoke(expression, argument);
        var right = Expression.Constant(filter);

        var eq = comparisonType switch
        {
            ComparisonType.Equal => Expression.Equal(left, right),
            ComparisonType.NotEqual => Expression.NotEqual(left, right),
            ComparisonType.GreaterThan => Expression.GreaterThan(left, right),
            ComparisonType.GreaterThanOrEqual => Expression.GreaterThanOrEqual(left, right),
            ComparisonType.LessThan => Expression.LessThan(left, right),
            ComparisonType.LessThanOrEqual => Expression.LessThanOrEqual(left, right),
            _ => throw new NotImplementedException($"Unsupported Comparison Type: '{comparisonType}'")
        };

        var lambda = Expression
            .Lambda<Func<TSource, bool>>(eq, new[] { argument });

        return source.Where(lambda);
    }
}
