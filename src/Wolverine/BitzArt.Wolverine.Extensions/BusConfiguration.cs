using Wolverine;
using Wolverine.AzureServiceBus;
using Wolverine.RabbitMQ.Internal;

namespace BitzArt;

public interface IBusConfiguration
{
    internal Action<WolverineOptions>? CommonConfiguration { get; set; }
    internal Dictionary<BusType, Action<WolverineOptions, object>> ImplementationConfiguration { get; }
}

internal class BusConfiguration : IBusConfiguration
{
    public Action<WolverineOptions>? CommonConfiguration { get; set; }
    public Dictionary<BusType, Action<WolverineOptions, object>> ImplementationConfiguration { get; }

    public BusConfiguration()
    {
        CommonConfiguration = null;
        ImplementationConfiguration = [];
    }
}

public static class BusConfigurationExtensions
{
    public static IBusConfiguration Configure(this IBusConfiguration configuration, Action<WolverineOptions> configure)
    {
        configuration.CommonConfiguration = configure;
        return configuration;
    }
}

public static class BusConfigurationImplementationExtensions
{
    public static IBusConfiguration Configure<TImplementationConfiguration>(this IBusConfiguration configuration, BusType busType, Action<WolverineOptions, TImplementationConfiguration> configurationAction)
    {
        configuration.ImplementationConfiguration[busType] = (options, o) =>
        {
            configurationAction(options, (TImplementationConfiguration)o);
        };
        
        return configuration;
    }

    public static IBusConfiguration ConfigureRabbitMq(this IBusConfiguration configuration, Action<WolverineOptions, RabbitMqTransportExpression> configure)
        => configuration.Configure(BusType.RabbitMQ, configure);

    public static IBusConfiguration ConfigureAzureServiceBus(this IBusConfiguration configuration, Action<WolverineOptions, AzureServiceBusConfiguration> configure)
        => configuration.Configure(BusType.AzureServiceBus, configure);
}