namespace BitzArt;

public record AzureServiceBusTransportConfiguration
{
    /// <summary>
    /// Name of the transport configuration.
    /// </summary>
    public required string? Name { get; init; }
    
    /// <summary>
    /// Azure Service Bus connection string.
    /// </summary>
    public required string ConnectionString { get; init; }
    
    /// <summary>
    /// Prefetch count for the message bus.
    /// </summary>
    public int? PrefetchCount { get; set; } = null;
}