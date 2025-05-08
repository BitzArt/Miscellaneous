namespace BitzArt;

/// <summary>
/// Options for interservice messaging.
/// </summary>
public abstract record TransportConfiguration
{
    /// <summary>
    /// Name for identifying the messaging configuration.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Prefetch count for the message bus.
    /// </summary>
    public required int? PrefetchCount { get; init; } = null;
}