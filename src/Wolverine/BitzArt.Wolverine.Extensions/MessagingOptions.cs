using JasperFx.Core;

namespace BitzArt;

/// <summary>
/// Options for interservice messaging.
/// </summary>
public class MessagingOptions
{
    /// <summary>
    /// Configuration section name.
    /// </summary>
    public const string SectionName = "Messaging";

    /// <summary>
    /// Type of the message bus.
    /// </summary>
    public BusType BusType => Type.ToEnum<BusType>();

    /// <summary>
    /// Type of the message bus.
    /// </summary>
    public required string Type { get; set; }

    /// <summary>
    /// Prefetch count for the message bus.
    /// </summary>
    public int? PrefetchCount { get; set; } = null;

    // --- Azure Service Bus ---

    /// <summary>
    /// Azure Service Bus connection string.
    /// </summary>
    public string? ConnectionString { get; set; }

    // --- RabbitMQ ---

    /// <summary>
    /// RabbitMQ connection host.
    /// </summary>
    public string? Host { get; set; }

    /// <summary>
    /// RabbitMQ connection username.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// RabbitMQ connection password.
    /// </summary>
    public string? Password { get; set; }
}
