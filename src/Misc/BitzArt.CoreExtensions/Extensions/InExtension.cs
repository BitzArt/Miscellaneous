namespace BitzArt;

/// <summary>
/// In extension method for comparable struct values.
/// </summary>
public static class InExtension
{
    /// <summary>
    /// Determines whether the specified value is within the given <see cref="Range{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value and range bounds.</typeparam>
    /// <param name="value">The value to check.</param>
    /// <param name="range">The range to check against.</param>
    /// <returns><see langword="true"/> if the value is within the given range, otherwise <see langword="false"/>.</returns>
    public static bool In<T>(this T value, Range<T> range)
        where T : struct, IComparable<T>
    {
        if (!range.LowerBound.HasValue && !range.UpperBound.HasValue) 
            return true;

        if (range.LowerBound.HasValue)
        {
            var startComparisonResult = value.CompareTo(range.LowerBound.Value);
            var belowStart = range.IncludeLowerBound ? startComparisonResult < 0 : startComparisonResult <= 0;
            if (belowStart) return false;
        }

        if (range.UpperBound.HasValue)
        {
            var endComparisonResult = value.CompareTo(range.UpperBound.Value);
            var aboveEnd = range.IncludeUpperBound ? endComparisonResult > 0 : endComparisonResult >= 0;
            if (aboveEnd) return false;
        }

        return true;
    }
}
