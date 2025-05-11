namespace BitzArt;

public record RabbitMqTransportConfiguration
{
    /// <summary>
    /// Name of the transport configuration.
    /// </summary>
    public required string? Name { get; init; }
    
    /// <summary>
    /// RabbitMQ connection host.
    /// </summary>
    public required string? Host { get; init; }

    /// <summary>
    /// RabbitMQ connection username.
    /// </summary>
    public required string? Username { get; init; }

    /// <summary>
    /// RabbitMQ connection password.
    /// </summary>
    public required string? Password { get; init; }
    
    /// <summary>
    /// Prefetch count for the message bus.
    /// </summary>
    public int? PrefetchCount { get; set; } = null;
}