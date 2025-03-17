namespace BitzArt;

/// <summary>
/// Represents a range within specified bounds.
/// </summary>
/// <typeparam name="T">The type of bound values.</typeparam>
public record struct Range<T>
{
    /// <summary>
    /// The lower bound of the range.
    /// </summary>
    /// <remarks>
    /// If <typeparamref name="T"/> implements <see cref="IComparable"/> and 
    /// both <see cref="LowerBound"/> and <see cref="UpperBound"/> are provided, correct boundary order will be ensured (boundary values may be swapped).
    /// </remarks>
    public T LowerBound
    {
        get => _lowerBound;
        set
        {
            _lowerBound = value;
            EnsureBoundsOrder();
        }
    }

    private bool _hasLowerBound => _isNullable == false || _lowerBound is not null;

    private bool _isNullable;

    private bool _isComparable;

    private T _lowerBound;

    /// <summary>
    /// The upper bound of the range.
    /// </summary>
    /// <remarks>
    /// If <typeparamref name="T"/> implements <see cref="IComparable"/> and 
    /// both <see cref="LowerBound"/> and <see cref="UpperBound"/> are provided, correct boundary order will be ensured (boundary values may be swapped).
    /// </remarks>
    public T UpperBound
    {
        get => _upperBound;
        set
        {
            _upperBound = value;
            EnsureBoundsOrder();
        }
    }

    private bool _hasUpperBound => _isNullable == false || _upperBound is not null;

    private T _upperBound;

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
            if (!_hasLowerBound)
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
            if (!_hasUpperBound)
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
    /// If <typeparamref name="T"/> implements <see cref="IComparable"/> and 
    /// both <paramref name="lowerBound"/> and <paramref name="upperBound"/> are provided, correct boundary order is ensured (boundary values may be swapped).
    /// </remarks>
    /// <param name="lowerBound">The lower bound of the range.</param>
    /// <param name="upperBound">The upper bound of the range.</param>
    /// <param name="includeLowerBound">Whether the lower bound is included in the range.</param>
    /// <param name="includeUpperBound">Whether the upper bound is included in the range.</param>
    public Range(T lowerBound, T upperBound, bool includeLowerBound = true, bool includeUpperBound = true)
    {
        _isNullable = typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>);

        var underlyingType = _isNullable ? Nullable.GetUnderlyingType(typeof(T))! : typeof(T);
        _isComparable = underlyingType.GetInterfaces().Contains(typeof(IComparable));

        _lowerBound = lowerBound;
        _upperBound = upperBound;
        _includeStart = includeLowerBound;
        _includeEnd = includeUpperBound;

        EnsureBoundsOrder();
    }

    private void EnsureBoundsOrder()
    {
        // Does not implement IComparable, so cannot compare the bounds.
        if (!_isComparable) return;

        // If the bounds are nullable and at least one of them is null, no need to compare them.
        if (_isNullable && (_lowerBound is null || _upperBound is null))
            return;

        var inOrder = ((IComparable)_lowerBound!).CompareTo((IComparable)_upperBound!) <= 0;
        if (inOrder) return;

        (_lowerBound, _upperBound) = (_upperBound, _lowerBound);
        (_includeStart, _includeEnd) = (_includeEnd, _includeStart);
    }

    /// <summary>
    /// Returns a string representation of this <see cref="Range{T}"/>.
    /// </summary>
    public override string ToString()
    {
        var openingBracket = _hasLowerBound ? IncludeLowerBound ? '[' : '(' : '(';
        var lowerBound = LowerBound?.ToString() ?? "unlimited";

        var upperBound = UpperBound?.ToString() ?? "unlimited";
        var closingBracket = _hasUpperBound ? IncludeUpperBound ? ']' : ')' : ')';

        return $"{openingBracket}{lowerBound}, {upperBound}{closingBracket}";
    }
}
