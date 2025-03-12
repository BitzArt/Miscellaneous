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
    /// If <see cref="LowerBound"/> is greater than <see cref="UpperBound"/>, their values will be automatically swapped.
    /// </remarks>
    public T? LowerBound
    {
        get => _lowerBound;
        set
        {
            _lowerBound = value;
            EnsureBoundsOrder();
        }
    }

    private T? _lowerBound;

    /// <summary>
    /// The upper bound of the range.
    /// </summary>
    /// <remarks>
    /// If <see cref="UpperBound"/> is less than <see cref="LowerBound"/>, their values will be automatically swapped.
    /// </remarks>
    public T? UpperBound
    {
        get => _upperBound;
        set
        {
            _upperBound = value;
            EnsureBoundsOrder();
        }
    }

    private T? _upperBound;

    /// <summary>
    /// Whether the lower bound is included in the range.
    /// </summary>
    /// <remarks>
    /// Default value is <see langword="true"/>.<br/>
    /// Always equals to <see langword="false"/> when the lower bound is <see langword="null"/>.
    /// </remarks>
    public bool IncludeLowerBound
    {
        get
        {
            if (!_lowerBound.HasValue)
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
    public bool IncludeUpperBound
    {
        get
        {
            if (!_upperBound.HasValue)
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
    /// If <paramref name="lowerBound"/> is greater than <paramref name="upperBound"/>, their values will be automatically swapped.
    /// </remarks>
    /// <param name="lowerBound">The lower bound of the range.</param>
    /// <param name="upperBound">The upper bound of the range.</param>
    /// <param name="includeLowerBound">Whether the lower bound is included in the range.</param>
    /// <param name="includeUpperBound">Whether the upper bound is included in the range.</param>
    public Range(T? lowerBound, T? upperBound, bool includeLowerBound = true, bool includeUpperBound = true)
    {
        _lowerBound = lowerBound;
        _upperBound = upperBound;
        _includeStart = includeLowerBound;
        _includeEnd = includeUpperBound;
        EnsureBoundsOrder();
    }

    private void EnsureBoundsOrder()
    {
        if (!_lowerBound.HasValue || !_upperBound.HasValue)
            return;

        var inOrder = _lowerBound.Value.CompareTo(_upperBound.Value) <= 0;
        if (inOrder) return;

        (_lowerBound, _upperBound) = (_upperBound, _lowerBound);
        (_includeStart, _includeEnd) = (_includeEnd, _includeStart);
    }

    /// <summary>
    /// Returns a string representation of this <see cref="Range{T}"/>.
    /// </summary>
    public override string ToString()
    {
        var openingBracket = LowerBound.HasValue ? IncludeLowerBound ? "[" : "(" : "(";
        var lowerBound = LowerBound?.ToString() ?? "−∞";

        var upperBound = UpperBound?.ToString() ?? "+∞";
        var closingBracket = UpperBound.HasValue ? IncludeUpperBound ? "]" : ")" : ")";

        return $"{openingBracket}{lowerBound}, {upperBound}{closingBracket}";
    }
}
