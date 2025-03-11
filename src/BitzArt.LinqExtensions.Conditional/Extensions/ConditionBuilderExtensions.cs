using System.Linq.Expressions;

namespace BitzArt.Linq.Conditional;

/// <summary>
/// Condition builder extensions for LINQ queries.
/// </summary>
public static class ConditionBuilderExtensions
{
    /// <summary>
    /// Marks the beginning of a complex 'Where' condition.
    /// </summary>
    public static IWhereConditionBuilder<TSource, TMember> Where<TSource, TMember>(this IQueryable<TSource> source, Expression<Func<TSource, TMember>> memberSelector)
    {
        return new WhereConditionBuilder<TSource, TMember>(source, memberSelector);
    }

    /// <summary>
    /// Filters a sequence of values based on a predicate applied to the selected member.
    /// </summary>
    public static IWhereConditionBuilder<TSource, TMember> IsTrue<TSource, TMember>(this IWhereConditionBuilder<TSource, TMember> builder, Expression<Func<TMember, bool>> conditionExpression)
    {
        var selectorExpression = builder.Selector;
        var operationExpression = conditionExpression;

        var expression = Expression.Lambda<Func<TSource, bool>>(
            Expression.Invoke(operationExpression, selectorExpression.Body),
            selectorExpression.Parameters
        );

        builder.Query = builder.Query.Where(expression);

        return builder;
    }
}
