namespace BitzArt;

public record AzureServiceBusTransportConfiguration
{
    /// <summary>
    /// Azure Service Bus connection string.
    /// </summary>
    public required string ConnectionString { get; init; }
}