namespace BitzArt;

/// <summary>
/// Extension methods for <see cref="Range{T}"/>.
/// </summary>
public static class RangeExtensions
{
    /// <summary>
    /// Determines whether the specified value is within the given <see cref="Range{T}"/>'s boundaries.
    /// </summary>
    /// <typeparam name="T">The type of the value and range bounds.</typeparam>
    /// <param name="value">The value to check.</param>
    /// <param name="range">The range to check against.</param>
    /// <returns><see langword="true"/> if the value is within the given range's boundaries, otherwise <see langword="false"/>.</returns>
    public static bool Contains<T>(this Range<T?> range, T value)
        where T : struct, IComparable<T>
    {
        return Contains(value, range.LowerBound, range.UpperBound, range.IncludeLowerBound, range.IncludeUpperBound);
    }

    private static bool Contains<T>(T value, T? lowerBound, T? upperBound, bool includeLowerBound, bool includeUpperBound)
    where T : struct, IComparable<T>
    {
        if (lowerBound is not null)
        {
            var startComparisonResult = value.CompareTo(lowerBound!.Value);
            var belowStart = includeLowerBound ? startComparisonResult < 0 : startComparisonResult <= 0;
            if (belowStart) return false;
        }

        if (upperBound is not null)
        {
            var endComparisonResult = value.CompareTo(upperBound!.Value);
            var aboveEnd = includeUpperBound ? endComparisonResult > 0 : endComparisonResult >= 0;
            if (aboveEnd) return false;
        }

        return true;
    }
}
