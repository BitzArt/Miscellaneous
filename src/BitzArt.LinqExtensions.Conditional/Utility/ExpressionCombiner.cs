using System.Linq.Expressions;

namespace System.Linq;

internal static class ExpressionCombiner
{
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> exp, Expression<Func<T, bool>> newExp)
    {
        var visitor = new ParameterUpdateVisitor(newExp.Parameters.First(), exp.Parameters.First());
        newExp = (visitor.Visit(newExp) as Expression<Func<T, bool>>)!;

        var binExp = Expression.And(exp.Body, newExp.Body);
        return Expression.Lambda<Func<T, bool>>(binExp, newExp.Parameters);
    }

    private class ParameterUpdateVisitor(ParameterExpression oldParameter, ParameterExpression newParameter) : ExpressionVisitor
    {
        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (object.ReferenceEquals(node, oldParameter))
                return newParameter;

            return base.VisitParameter(node);
        }
    }
}
