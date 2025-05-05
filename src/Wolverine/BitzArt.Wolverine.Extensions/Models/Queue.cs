namespace BitzArt;

/// <summary>
/// Represents a queue in the messaging system.
/// </summary>
public sealed record Queue
{
    /// <summary>
    /// The name of the queue.
    /// </summary>
    public required string Name { get; init; }
}