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
    
    private static AzureServiceBusOptions GetRabbitMqOptions(string name, IConfiguration configuration)
    {
        var section = configuration
            .GetSection("Messaging")
            .GetChildren()
            .SingleOrDefault(section => section["Name"] == name && section["Type"] == AzureServiceBusOptions.BusType);

        if (section == null)
        {
            throw new InvalidOperationException($"No configuration found for bus '{name}'");
        }

        var rabbitMqOptions = new AzureServiceBusOptions
        {
            Name = section["Name"]!,
            PrefetchCount = section.GetValue<int?>("PrefetchCount"),
            ConnectionString = section["ConnectionString"]!,
        };
        return rabbitMqOptions;
    }
}