namespace BitzArt;

/// <summary>
/// Extension methods for merging ranges.
/// </summary>
public static class RangeMergeExtensions
{
    /// <inheritdoc cref="Merge{T}(IEnumerable{Range{T?}})"/>
    public static ICollection<Range<T?>> Merge<T>(this ICollection<Range<T?>> ranges)
        where T : struct, IComparable<T>
        => ((IEnumerable<Range<T?>>)ranges).Merge().ToList();

    /// <summary>
    /// Merges the given ranges.
    /// </summary>
    /// <typeparam name="T">Range bound type.</typeparam>
    /// <param name="ranges">The ranges to merge.</param>
    /// <returns>A minimal set of non-overlapping ranges.</returns>
    public static IEnumerable<Range<T?>> Merge<T>(this IEnumerable<Range<T?>> ranges)
        where T : struct, IComparable<T>
        => RangeMergeUtility.Merge(ranges);
}
