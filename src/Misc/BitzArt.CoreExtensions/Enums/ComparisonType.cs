namespace BitzArt;

/// <summary>
/// Condition for comparing values.
/// </summary>
public enum ComparisonType : byte
{
    /// <summary>
    /// Represents an equality condition.
    /// </summary>
    Equal = 0,

    /// <summary>
    /// Represents an inequality condition.
    /// </summary>
    NotEqual = 1,

    /// <summary>
    /// Represents a 'greater than' condition.
    /// </summary>
    GreaterThan = 2,

    /// <summary>
    /// Represents a 'greater than or equal' condition.
    /// </summary>
    GreaterThanOrEqual = 3,

    /// <summary>
    /// Represents a 'less than' condition.
    /// </summary>
    LessThan = 4,

    /// <summary>
    /// Represents a 'less than or equal' condition.
    /// </summary>
    LessThanOrEqual = 5
}
