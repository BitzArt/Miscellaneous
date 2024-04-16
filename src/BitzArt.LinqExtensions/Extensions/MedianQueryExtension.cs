using System;
using System.Linq;
using System.Linq.Expressions;

namespace BitzArt;

/// <summary>
/// Extension methods for median calculation in LINQ queries.
/// </summary>
public static class MedianQueryExtension
{
    /// <summary>
    /// Keep in mind that this implementation is not a fully correct median calculation. <br />
    /// Normally, when the number of elements is even, the median is calculated by taking the average of the two middle elements. <br />
    /// This implementation simply uses the second element of the two, without any averaging. <br />
    /// This is done to simplify the implementation, improve performance, and allow <c>TValue</c>s that don't support <c>Average()</c>. <br />
    /// On large enough data sets, the difference between the two methods is negligible. <br />
    /// You should implement your own median calculation if you need a more mathematically precise result.
    /// </summary>
    /// <returns>The median value of the value expression.</returns>
    public static IQueryable<TValue> Median<TSource, TValue>(this IQueryable<TSource> source, Expression<Func<TSource, TValue>> valueExpression)
    {
        return source
            .OrderBy(valueExpression)
            .Skip(source.Count() / 2)
            .Take(1)
            .Select(valueExpression);
    }
}
