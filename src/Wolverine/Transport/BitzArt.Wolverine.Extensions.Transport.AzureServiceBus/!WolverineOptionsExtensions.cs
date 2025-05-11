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
        var transportConfiguration = configuration
            .GetRequiredSection("Messaging:0")
            .Get<AzureServiceBusTransportConfiguration>()!;

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