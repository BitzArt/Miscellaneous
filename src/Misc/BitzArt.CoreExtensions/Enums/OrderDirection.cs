namespace BitzArt;

/// <summary>
/// Order direction (ascending/descending).
/// </summary>
public enum OrderDirection : byte
{
    /// <summary>
    /// Ascending order direction.
    /// </summary>
    Ascending = 0,

    /// <summary>
    /// Descending order direction.
    /// </summary>
    Descending = 1
}

/// <summary>
/// Extensions to <see cref="OrderDirection"/>.
/// </summary>
public static class OrderDirectionExtensions
{
    /// <summary>
    /// Reverse the order direction.
    /// </summary>
    /// <param name="orderDirection"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static OrderDirection Reverse(this OrderDirection orderDirection)
    {
        return orderDirection switch
        {
            OrderDirection.Ascending => OrderDirection.Descending,
            OrderDirection.Descending => OrderDirection.Ascending,
            _ => throw new ArgumentOutOfRangeException(nameof(orderDirection), orderDirection, null)
        };
    }
}
