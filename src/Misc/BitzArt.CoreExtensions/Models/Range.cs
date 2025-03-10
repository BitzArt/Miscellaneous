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
    /// Default value is <see langword="true"/>.<br/>
    /// Always equals to <see langword="false"/> when the lower bound is <see langword="null"/>.
    /// </remarks>
    public bool IncludeStart
    {
        get
        {
            if (!_start.HasValue)
                return false; // If the lower bound is null, it cannot be included in the range.

            return _includeStart;
        }

        set => _includeStart = value;
    }

    private bool _includeStart = true;

    /// <summary>
    /// Whether the upper bound is included in the range.
    /// </summary>
    /// <remarks>
    /// Default value is <see langword="true"/>.<br/>
    /// Always equals to <see langword="false"/> when the upper bound is <see langword="null"/>.
    /// </remarks>
    public bool IncludeEnd
    {
        get
        {
            if (!_end.HasValue)
                return false; // If the upper bound is null, it cannot be included in the range.

            return _includeEnd;
        }

        set => _includeEnd = value;
    }

    private bool _includeEnd = true;

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
        _includeStart = includeStart;
        _includeEnd = includeEnd;
        EnsureBoundsOrder();
    }

    private void EnsureBoundsOrder()
    {
        if (!_start.HasValue || !_end.HasValue)
            return;

        var inOrder = _start.Value.CompareTo(_end.Value) <= 0;
        if (inOrder) return;

        (_start, _end) = (_end, _start);
        (_includeStart, _includeEnd) = (_includeEnd, _includeStart);
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
