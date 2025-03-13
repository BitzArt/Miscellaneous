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
}
