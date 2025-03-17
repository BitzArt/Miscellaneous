using System.Linq.Expressions;

namespace BitzArt.Extensions;

/// <summary>
/// Expression extensions.
/// </summary>
public static class ExpressionExtensions
{
    /// <summary>
    /// Applies the given predicate to the target expression.
    /// </summary>
    public static Expression<Func<TSource, bool>> Apply<TSource, TTarget>(this Expression<Func<TTarget, bool>> predicate, Expression<Func<TSource, TTarget>> targetExpression)
    {
        return Expression.Lambda<Func<TSource, bool>>(
            Expression.Invoke(predicate, targetExpression.Body),
            targetExpression.Parameters
        );
    }

    /// <summary>
    /// Combines two predicate expressions with an OR operation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="left">First predicate.</param>
    /// <param name="right">Second predicate.</param>
    /// <returns>A new predicate that combines the two predicates with an OR operation.</returns>
    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        var parameter = Expression.Parameter(typeof(T));
        var leftBody = Expression.Invoke(left, parameter);
        var rightBody = Expression.Invoke(right, parameter);
        var body = Expression.OrElse(leftBody, rightBody);
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}
