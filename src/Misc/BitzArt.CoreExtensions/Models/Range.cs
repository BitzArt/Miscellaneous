namespace BitzArt;

/// <summary>
/// Represents a range within specified bounds.
/// </summary>
/// <typeparam name="T">The type of bound values.</typeparam>
public record struct Range<T>
    where T : struct, IComparable<T>
{
    /// <summary>
    /// The lower bound of the range.
    /// </summary>
    /// <remarks>
    /// If <see cref="Start"/> is greater than <see cref="End"/>, their values will be automatically swapped.
    /// </remarks>
    public T? Start
    {
        get => _start;
        set
        {
            _start = value;
            EnsureBoundsOrder();
        }
    }

    private T? _start;

    /// <summary>
    /// The upper bound of the range.
    /// </summary>
    /// <remarks>
    /// If <see cref="End"/> is less than <see cref="Start"/>, their values will be automatically swapped.
    /// </remarks>
    public T? End
    {
        get => _end;
        set
        {
            _end = value;
            EnsureBoundsOrder();
        }
    }

    private T? _end;

    /// <summary>
    /// Whether the lower bound is included in the range.
    /// </summary>
    /// <remarks>
    /// Default value is <see langword="true"/>.
    /// </remarks>
    public bool IncludeStart { get; set; } = true;

    /// <summary>
    /// Whether the upper bound is included in the range.
    /// </summary>
    /// <remarks>
    /// Default value is <see langword="true"/>.
    /// </remarks>
    public bool IncludeEnd { get; set; } = true;

    /// <summary>
    /// Initializes a new instance of the <see cref="Range{T}"/>.
    /// </summary>
    /// <remarks>
    /// If <paramref name="start"/> is greater than <paramref name="end"/>, their values will be automatically swapped.
    /// </remarks>
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
        EnsureBoundsOrder();
    }

    private void EnsureBoundsOrder()
    {
        if (!_start.HasValue || !_end.HasValue)
            return;

        var inOrder = _start.Value.CompareTo(_end.Value) <= 0;
        if (inOrder) return;

        (_start, _end) = (_end, _start);
        (IncludeStart, IncludeEnd) = (IncludeEnd, IncludeStart);
    }

    /// <summary>
    /// Returns a string representation of this <see cref="Range{T}"/>.
    /// </summary>
    public override string ToString()
    {
        var openingBracket = Start.HasValue ? IncludeStart ? "[" : "(" : "(";
        var lowerBound = Start?.ToString() ?? "−∞";

        var upperBound = End?.ToString() ?? "+∞";
        var closingBracket = End.HasValue ? IncludeEnd ? "]" : ")" : ")";

        return $"{openingBracket}{lowerBound}, {upperBound}{closingBracket}";
    }
}
