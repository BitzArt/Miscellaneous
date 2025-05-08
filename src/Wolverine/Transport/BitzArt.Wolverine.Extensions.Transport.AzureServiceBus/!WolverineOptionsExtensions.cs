using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Wolverine;
using Wolverine.AzureServiceBus;

namespace BitzArt;

public static class WolverineOptionsExtensions
{
    public static void ConfigureAzureServiceBusTransport(
        this WolverineOptions options,
        IConfiguration configuration,
        Action<WolverineOptions, object> implementationConfiguration)
    {
        var transportConfigurations = configuration.GetAzureServiceBusTransportConfigurations();
        
        // Currently, only one configuration is supported
        var transportConfiguration = transportConfigurations.First();
        
        options.ConfigureAzureServiceBusTransport(transportConfiguration, implementationConfiguration);
    }
    
    public static void ConfigureAzureServiceBusTransport(
        this WolverineOptions options,
        AzureServiceBusTransportConfiguration transportConfiguration,
        Action<WolverineOptions, object> implementationConfiguration)
    {
        var azureServiceBus = options.UseAzureServiceBus(transportConfiguration.ConnectionString)

            // Let Wolverine try to initialize any missing queues
            // on the first usage at runtime
            .AutoProvision()

            // This enables Wolverine to use temporary Azure Service Bus
            // queues created at runtime for communication between
            // Wolverine nodes
            .EnableWolverineControlQueues();

        implementationConfiguration.Invoke(options, azureServiceBus);
    }
}