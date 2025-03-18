using System.Diagnostics;

namespace BitzArt;

internal static class RangeMergeUtility
{
    public static IEnumerable<Range<T?>> Merge<T>(IEnumerable<Range<T?>> ranges)
        where T : struct, IComparable<T>
    {
        ArgumentNullException.ThrowIfNull(ranges);
        if (!ranges.Any()) return [];
        if (ranges.Count() == 1) return ranges;

        var stateChanges = new List<RangeMergeStateChange<T>>();

        foreach (var range in ranges)
        {
            stateChanges.Add(new(new(range.LowerBound, range.IncludeLowerBound), true));
            stateChanges.Add(new(new(range.UpperBound, range.IncludeUpperBound), false));
        }

        var openStateChanges = stateChanges.Where(x => x.Point.Position is null).ToList();
        var openStartStateChanges = new List<RangeMergeStateChange<T>>();
        var openEndStateChanges = new List<RangeMergeStateChange<T>>();

        foreach (var openStateChange in openStateChanges)
        {
            switch (openStateChange.Direction)
            {
                case true:
                    openStartStateChanges.Add(openStateChange);
                    break;
                case false:
                    openEndStateChanges.Add(openStateChange);
                    break;
            }
        }

        stateChanges = new List<RangeMergeStateChange<T>>(openStartStateChanges
            .Concat(stateChanges
                .Where(x => x.Point.Position is not null)
                .OrderBy(x => x.Point.Position))
            .Concat(openEndStateChanges));

        var currentState = 0;

        // changes where currentState goes from 0 to 1 or 1 to 0
        var extremeStateChanges = new List<RangeMergeStateChange<T>>();

        foreach (var stateChange in stateChanges)
        {
            switch (stateChange.Direction)
            {
                case true when currentState == 0:

                    extremeStateChanges.Add(stateChange);
                    currentState++;

                    TryMergeWithPrevious(extremeStateChanges, stateChange);

                    break;

                case true:
                    currentState++;
                    break;

                case false when currentState == 1:

                    extremeStateChanges.Add(stateChange);
                    currentState--;

                    TryMergeWithPrevious(extremeStateChanges, stateChange);

                    break;

                case false:
                    currentState--;
                    break;
            }
        }

        return BuildRangeMergeResult(extremeStateChanges);
    }

    private static void TryMergeWithPrevious<T>(List<RangeMergeStateChange<T>> extremeStateChanges, RangeMergeStateChange<T> next)
        where T : struct, IComparable<T>
    {
        if (extremeStateChanges.Count < 2) return;

        var previousStateChange = extremeStateChanges[^2];

        if (previousStateChange.Point.Position is null || next.Point.Position is null) return;

        if (previousStateChange.Point.Position!.Value.CompareTo(next.Point.Position!.Value) == 0)
        {
            if (previousStateChange.Direction == next.Direction)
                throw new UnreachableException("Unexpected: 2 extreme changes with same direction in a row.");

            if (previousStateChange.Point.Include || next.Point.Include)
            {
                // if at least one point includes bound,
                // merge the two points
                // by removing their respective extreme state changes
                extremeStateChanges.RemoveAt(extremeStateChanges.Count - 1);
                extremeStateChanges.RemoveAt(extremeStateChanges.Count - 1);
            }
        }
    }

    private static List<Range<T?>> BuildRangeMergeResult<T>(List<RangeMergeStateChange<T>> extremeStateChanges)
        where T : struct, IComparable<T>
    {
        var result = new List<Range<T?>>();

        if (extremeStateChanges[0].Direction == false) extremeStateChanges.Insert(0, new(new(null, false), true));
        if (extremeStateChanges[^1].Direction == true) extremeStateChanges.Insert(extremeStateChanges.Count, new(new(null, false), false));

        for (var i = 0; i < extremeStateChanges.Count - 1; i += 2)
        {
            var openPoint = extremeStateChanges[i].Point;
            var closePoint = extremeStateChanges[i + 1].Point;

            result.Add(new(openPoint.Position, closePoint.Position, openPoint.Include, closePoint.Include));
        }

        return result;
    }

    private record struct RangeMergeStateChange<T>(RangeEndpoint<T> Point, bool Direction)
        where T : struct, IComparable<T>;

    private record struct RangeEndpoint<T>(T? Position, bool Include)
        where T : struct, IComparable<T>;
}
