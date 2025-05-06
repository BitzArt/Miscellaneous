namespace BitzArt.Wolverine.Extensions.AzureServiceBus;

public sealed record AzureServiceBusOptions : MessagingOptions
{
    /// <summary>
    /// Azure Service Bus connection string.
    /// </summary>
    public required string ConnectionString { get; init; }

    public override string BusType => "AzureServiceBus";
}