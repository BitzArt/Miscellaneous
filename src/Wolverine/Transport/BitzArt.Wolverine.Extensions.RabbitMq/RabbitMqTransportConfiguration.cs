namespace BitzArt.Wolverine.Extensions.RabbitMq;

public sealed record RabbitMqTransportConfiguration : TransportConfiguration
{
    public const string BusType = "RabbitMQ";
    
    /// <summary>
    /// RabbitMQ connection host.
    /// </summary>
    public required string Host { get; init; }

    /// <summary>
    /// RabbitMQ connection username.
    /// </summary>
    public required string Username { get; init; }

    /// <summary>
    /// RabbitMQ connection password.
    /// </summary>
    public required string Password { get; init; }
}