namespace BitzArt;

/// <summary>
/// Represents a range within specified boundaries.
/// </summary>
/// <typeparam name="T">The type of bound values.</typeparam>
public record struct Range<T>
    where T : struct, IComparable<T>
{
    /// <summary>
    /// The lower bound of the range.
    /// </summary>
    public T? Start
    {
        get => _start;
        set
        {
            _start = value;
            EnsureOrder();
        }
    }

    private T? _start;

    /// <summary>
    /// The upper bound of the range.
    /// </summary>
    public T? End
    {
        get => _end;
        set
        {
            _end = value;
            EnsureOrder();
        }
    }

    private T? _end;

    /// <summary>
    /// Whether the lower boundary is included in the range.
    /// </summary>
    public bool IncludeStart { get; set; }

    /// <summary>
    /// Whether the upper boundary is included in the range.
    /// </summary>
    public bool IncludeEnd { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Range{T}"/>.
    /// </summary>
    /// <param name="start">The lower bound of the range.</param>
    /// <param name="end">The upper bound of the range.</param>
    /// <param name="includeStart">Whether the lower bound is included in the range.</param>
    /// <param name="includeEnd">Whether the upper bound is included in the range.</param>
    public Range(T? start, T? end, bool includeStart = true, bool includeEnd = true)
    {
        _start = start;
        _end = end;
        IncludeStart = includeStart;
        IncludeEnd = includeEnd;
        EnsureOrder();
    }

    private void EnsureOrder()
    {
        if (_start.HasValue && _end.HasValue)
        {
            var inOrder = _start.Value.CompareTo(_end.Value) <= 0;
            if (inOrder) return;

            (_start, _end) = (_end, _start);
            (IncludeStart, IncludeEnd) = (IncludeEnd, IncludeStart);
        }
    }
}
