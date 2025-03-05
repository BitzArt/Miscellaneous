namespace BitzArt;

/// <summary>
/// In extension for struct values.
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
        if (!range.Start.HasValue && !range.End.HasValue) 
            return true;

        if (range.Start.HasValue)
        {
            var startComparisonResult = value.CompareTo(range.Start.Value);
            var belowStart = range.IncludeStart ? startComparisonResult < 0 : startComparisonResult <= 0;
            if (belowStart) return false;
        }

        if (range.End.HasValue)
        {
            var endComparisonResult = value.CompareTo(range.End.Value);
            var aboveEnd = range.IncludeEnd ? endComparisonResult > 0 : endComparisonResult >= 0;
            if (aboveEnd) return false;
        }

        return true;
    }
}
