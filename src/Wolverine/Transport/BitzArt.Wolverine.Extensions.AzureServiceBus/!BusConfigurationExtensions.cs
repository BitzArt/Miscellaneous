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
        var azureServiceBusOptions = GetAzureServiceBusOptions(bus.Name, configuration);

        var azureServiceBus = wolverineOptions
            .UseAzureServiceBus(azureServiceBusOptions.ConnectionString!)

            // Let Wolverine try to initialize any missing queues
            // on the first usage at runtime
            .AutoProvision()

            // This enables Wolverine to use temporary Azure Service Bus
            // queues created at runtime for communication between
            // Wolverine nodes
            .EnableWolverineControlQueues();

        configure?.Invoke(azureServiceBus);
        
        return bus;
    }
    
    private static AzureServiceBusTransportConfiguration GetAzureServiceBusOptions(string name, IConfiguration configuration)
    {
        var section = configuration
            .GetSection("Messaging")
            .GetChildren()
            .SingleOrDefault(section => section["Name"] == name && section["Type"] == AzureServiceBusTransportConfiguration.BusType);

        if (section == null)
        {
            throw new InvalidOperationException($"No configuration found for bus '{name}'");
        }

        var rabbitMqOptions = new AzureServiceBusTransportConfiguration
        {
            Name = section["Name"]!,
            PrefetchCount = section.GetValue<int?>("PrefetchCount"),
            ConnectionString = section["ConnectionString"]!,
        };
        
        return rabbitMqOptions;
    }
}