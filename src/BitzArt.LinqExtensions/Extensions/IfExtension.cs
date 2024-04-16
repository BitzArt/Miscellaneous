using System.Linq.Expressions;

namespace System.Linq;

/// <summary>
/// If extension for LINQ queries.
/// </summary>
public static class IfExtension
{
    /// <summary>
    /// Executes the expression on the source if the condition is true.
    /// </summary>
    public static IQueryable<T> If<T>(this IQueryable<T> source, bool condition, Expression<Func<IQueryable<T>, IQueryable<T>>> expression)
    {
        if (condition) return expression.Compile().Invoke(source);
        return source;
    }
}
