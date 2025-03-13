using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace BitzArt.Extensions;

/// <summary>
/// Extension methods for <see cref="Range{T}"/>.
/// </summary>
public static class RangeExtensions
{
    /// <summary>
    /// Returns an expression that checks if the value is within the given range.
    /// </summary>
    /// <typeparam name="TSource">Queryable data type.</typeparam>
    /// <typeparam name="TTarget">The type of the value and range bounds.</typeparam>
    /// <param name="range">The range to check against.</param>
    /// <param name="targetExpression">The expression that selects the value to check.</param>
    /// <returns> An expression that checks if the value is within the given range. </returns>
    public static Expression<Func<TSource, bool>> GetInclusionExpression<TSource, TTarget>(
        this Range<TTarget> range,
        Expression<Func<TSource, TTarget>> targetExpression)
        where TTarget : struct, IComparable<TTarget>
        => range.GetInclusionExpression().Apply(targetExpression);

    /// <inheritdoc cref="GetInclusionExpression{TSource, TTarget}(Range{TTarget}, Expression{Func{TSource, TTarget}})"/>
    [SuppressMessage("Style", "IDE0075:Simplify conditional expression")]
    public static Expression<Func<T, bool>> GetInclusionExpression<T>(this Range<T> range)
        where T : struct, IComparable<T>
        => x =>
        (
        range.LowerBound.HasValue
            ? range.IncludeLowerBound
                ? range.LowerBound!.Value.CompareTo(x) <= 0
                : range.LowerBound!.Value.CompareTo(x) < 0
            : true
        )
        &&
        (
        range.UpperBound.HasValue
            ? range.IncludeUpperBound
                ? range.UpperBound!.Value.CompareTo(x) >= 0
                : range.UpperBound!.Value.CompareTo(x) > 0
            : true
        );

    /// <summary>
    /// Returns an expression that checks if the value is within any of the given ranges.
    /// </summary>
    /// <typeparam name="TSource">Queryable data type.</typeparam>
    /// <typeparam name="TTarget">The type of the value and range bounds.</typeparam>
    /// <param name="ranges">A set of ranges to check against.</param>
    /// <param name="targetExpression">The expression that selects the value to check.</param>
    /// <returns> An expression that checks if the value is within any of the given ranges. </returns>
    public static Expression<Func<TSource, bool>> GetInclusionExpression<TSource, TTarget>(
        this IEnumerable<Range<TTarget>> ranges,
        Expression<Func<TSource, TTarget>> targetExpression)
        where TTarget : struct, IComparable<TTarget>
        => ranges.GetInclusionExpression().Apply(targetExpression);

    /// <inheritdoc cref="GetInclusionExpression{TSource, TTarget}(IEnumerable{Range{TTarget}}, Expression{Func{TSource, TTarget}})"/>/>
    [SuppressMessage("Style", "IDE0075:Simplify conditional expression")]
    public static Expression<Func<T, bool>> GetInclusionExpression<T>(this IEnumerable<Range<T>> ranges)
        where T : struct, IComparable<T>
        => x => ranges.Any(range => true);
        /*(
        range.LowerBound.HasValue
            ? range.IncludeLowerBound
                ? range.LowerBound!.Value.CompareTo(x) <= 0
                : range.LowerBound!.Value.CompareTo(x) < 0
            : true
        )
        &&
        (
        range.UpperBound.HasValue
            ? range.IncludeUpperBound
                ? range.UpperBound!.Value.CompareTo(x) >= 0
                : range.UpperBound!.Value.CompareTo(x) > 0
            : true
        ));*/
}
