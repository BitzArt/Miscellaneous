using MediaMars.Messaging;
using Microsoft.Extensions.Configuration;
using Wolverine;
using Wolverine.AzureServiceBus;

namespace BitzArt.Wolverine.Extensions.AzureServiceBus;

public static class BusConfigurationExtensions
{
    public static IBusConfiguration ConfigureAzureServiceBus(
        this IBusConfiguration bus,
        IConfiguration configuration,
        WolverineOptions wolverineOptions,
        Action<AzureServiceBusConfiguration>? configure)
    {
        return bus;
    }
}