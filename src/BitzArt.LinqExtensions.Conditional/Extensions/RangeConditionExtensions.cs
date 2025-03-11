using System.Linq.Expressions;

namespace BitzArt.Linq.Conditional;

/// <summary>
/// Extension methods for filtering by <see cref="Range{T}"/>
/// </summary>
public static class RangeConditionExtensions
{
    /// <summary>
    /// Filters a sequence by marking the member as being within the specified <see cref="Range{TMember}"/>
    /// </summary>
    public static IWhereConditionBuilder<TSource, TMember> IsIn<TSource, TMember>(this IWhereConditionBuilder<TSource, TMember> builder, Range<TMember>? range)
        where TMember : struct, IComparable<TMember>
    {
        if (range is null) return builder;

        var expression = GetRangeConstraintsExpression(range!.Value);
        if (expression is null) return builder;

        return builder.IsTrue(expression);
    }

    private static Expression<Func<TMember, bool>>? GetRangeConstraintsExpression<TMember>(Range<TMember> range)
        where TMember : struct, IComparable<TMember>
    {
        Expression<Func<TMember, bool>>? startConstraintExpression = GetRangeStartConstraintExpression(range);
        Expression<Func<TMember, bool>>? endConstraintExpression = GetRangeEndConstraintExpression(range);

        if (startConstraintExpression is null && endConstraintExpression is null) return null;

        if (startConstraintExpression is not null && endConstraintExpression is not null)
        {
            return startConstraintExpression.And(endConstraintExpression);
        }

        return startConstraintExpression ?? endConstraintExpression;
    }

    private static Expression<Func<TMember, bool>>? GetRangeStartConstraintExpression<TMember>(Range<TMember> range)
        where TMember : struct, IComparable<TMember>
    {
        if (range.Start is null) return null;

        var comparisonType = range.IncludeStart ? ComparisonType.GreaterThanOrEqual : ComparisonType.GreaterThan;

        return GetComparisonExpression(range.Start!.Value, comparisonType);
    }

    private static Expression<Func<TMember, bool>>? GetRangeEndConstraintExpression<TMember>(Range<TMember> range)
        where TMember : struct, IComparable<TMember>
    {
        if (range.End is null) return null;

        var comparisonType = range.IncludeEnd ? ComparisonType.LessThanOrEqual : ComparisonType.LessThan;

        return GetComparisonExpression(range.End!.Value, comparisonType);
    }

    private static Expression<Func<TMember, bool>> GetComparisonExpression<TMember>(TMember rangeValue, ComparisonType comparisonType)
        where TMember : struct, IComparable<TMember>
    {
        return comparisonType switch
        {
            ComparisonType.GreaterThan => member => member.CompareTo(rangeValue) > 0,
            ComparisonType.GreaterThanOrEqual => member => member.CompareTo(rangeValue) >= 0,
            ComparisonType.LessThan => member => member.CompareTo(rangeValue) < 0,
            ComparisonType.LessThanOrEqual => member => member.CompareTo(rangeValue) <= 0,
            _ => throw new NotSupportedException()
        };
    }
}
