namespace BitzArt.Wolverine.Extensions.AzureServiceBus;

public sealed record AzureServiceBusTransportConfiguration : TransportConfiguration
{
    public const string BusType = "AzureServiceBus";
    
    /// <summary>
    /// Azure Service Bus connection string.
    /// </summary>
    public required string ConnectionString { get; init; }
}