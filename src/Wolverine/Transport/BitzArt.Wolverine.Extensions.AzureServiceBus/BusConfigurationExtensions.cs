using MediaMars.Messaging;
using Wolverine.AzureServiceBus;

namespace BitzArt.Wolverine.Extensions.AzureServiceBus;

public static class BusConfigurationExtensions
{
    public static IBusConfiguration ConfigureAzureServiceBus(
        this IBusConfiguration busConfiguration,
        Action<AzureServiceBusConfiguration>? configure)
    {
        return busConfiguration;
    }
}