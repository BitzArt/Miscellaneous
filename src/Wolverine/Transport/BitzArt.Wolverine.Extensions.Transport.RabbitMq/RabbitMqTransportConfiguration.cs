namespace BitzArt;

public record RabbitMqTransportConfiguration
{
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
}