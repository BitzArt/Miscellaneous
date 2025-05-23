﻿using BitzArt.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace BitzArt;

/// <summary>
/// Extension methods for <see cref="Range{T}"/>.
/// </summary>
public static class RangeExtensions
{
    /// <summary>
    /// Determines whether the specified value lies within any of the given ranges' boundaries.
    /// </summary>
    /// <typeparam name="T">The type of the value and range bounds.</typeparam>
    /// <param name="value">The value to check.</param>
    /// <param name="ranges">The collection of ranges to check against.</param>
    /// <returns><see langword="true"/> if the value is within any of the given ranges' boundaries, otherwise <see langword="false"/>.</returns>
    public static bool Contains<T>(this IEnumerable<Range<T?>> ranges, T value)
        where T : struct, IComparable<T>
        => ranges.Any(range => range.Contains(value));

    /// <summary>
    /// Determines whether the specified value lies within the given range's boundaries.
    /// </summary>
    /// <typeparam name="T">The type of the value and range bounds.</typeparam>
    /// <param name="value">The value to check.</param>
    /// <param name="range">The range to check against.</param>
    /// <returns><see langword="true"/> if the value is within the given range's boundaries, otherwise <see langword="false"/>.</returns>
    public static bool Contains<T>(this Range<T?> range, T value)
        where T : struct, IComparable<T>
    {
        if (range.LowerBound is not null)
        {
            var startComparisonResult = value.CompareTo(range.LowerBound!.Value);
            var belowStart = range.IncludeLowerBound ? startComparisonResult < 0 : startComparisonResult <= 0;
            if (belowStart) return false;
        }
        if (range.UpperBound is not null)
        {
            var endComparisonResult = value.CompareTo(range.UpperBound!.Value);
            var aboveEnd = range.IncludeUpperBound ? endComparisonResult > 0 : endComparisonResult >= 0;
            if (aboveEnd) return false;
        }
        return true;
    }

    /// <summary>
    /// Returns an expression that checks if the value is within the given range.
    /// </summary>
    /// <typeparam name="TSource">Queryable data type.</typeparam>
    /// <typeparam name="TTarget">The type of the value and range bounds.</typeparam>
    /// <param name="range">The range to check against.</param>
    /// <param name="targetExpression">The expression that selects the value to check.</param>
    /// <returns> An expression that checks if the value is within the given range. </returns>
    public static Expression<Func<TSource, bool>> GetInclusionExpression<TSource, TTarget>(
        this Range<TTarget?> range,
        Expression<Func<TSource, TTarget>> targetExpression)
        where TTarget : struct, IComparable<TTarget>
        => range.GetInclusionExpression().Apply(targetExpression);

    /// <summary>
    /// Returns the query filtered by the given range applied to the given expression.
    /// </summary>
    /// <typeparam name="TSource">Queryable data type.</typeparam>
    /// <typeparam name="TTarget">The type of the value and range bounds.</typeparam>
    /// <param name="query">The queryable data source to filter.</param>
    /// <param name="targetExpression">The expression that selects the value to check.</param>
    /// <param name="range">The range to check against.</param>
    /// <returns>The query filtered by the given range applied to the given expression.</returns>
    public static IQueryable<TSource> Where<TSource, TTarget>(
        this IQueryable<TSource> query,
        Expression<Func<TSource, TTarget>> targetExpression,
        Range<TTarget?> range)
        where TTarget : struct, IComparable<TTarget>
        => query.Where(range.GetInclusionExpression(targetExpression));

    /// <inheritdoc cref="GetInclusionExpression{TSource, TTarget}(Range{TTarget?}, Expression{Func{TSource, TTarget}})"/>
    [SuppressMessage("Style", "IDE0075:Simplify conditional expression")]
    public static Expression<Func<T, bool>> GetInclusionExpression<T>(this Range<T?> range)
        where T : struct, IComparable<T>
        => x =>
        (
        range.LowerBound != null
            ? range.IncludeLowerBound
                ? range.LowerBound!.Value.CompareTo(x) <= 0
                : range.LowerBound!.Value.CompareTo(x) < 0
            : true
        )
        &&
        (
        range.UpperBound != null
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
        this IEnumerable<Range<TTarget?>> ranges,
        Expression<Func<TSource, TTarget>> targetExpression)
        where TTarget : struct, IComparable<TTarget>
        => ranges.GetInclusionExpression().Apply(targetExpression);

    /// <inheritdoc cref="GetInclusionExpression{TSource, TTarget}(IEnumerable{Range{TTarget?}}, Expression{Func{TSource, TTarget}})"/>/>
    [SuppressMessage("Style", "IDE0075:Simplify conditional expression")]
    public static Expression<Func<T, bool>> GetInclusionExpression<T>(this IEnumerable<Range<T?>> ranges)
        where T : struct, IComparable<T>
    {
        ArgumentNullException.ThrowIfNull(ranges);

        if (!ranges.Any()) return _ => true;

        var expressions = ranges.Select(range => range.GetInclusionExpression());
        return expressions.Aggregate((x, y) => x.Or(y));
    }
}
