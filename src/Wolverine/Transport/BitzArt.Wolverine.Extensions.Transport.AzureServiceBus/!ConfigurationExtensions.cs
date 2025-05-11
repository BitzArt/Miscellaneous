using Microsoft.Extensions.Configuration;

namespace BitzArt;

public static class ConfigurationExtensions
{
    public static IReadOnlyCollection<AzureServiceBusTransportConfiguration> GetAzureServiceBusTransportConfigurations(
        this IConfiguration configuration)
    {
        var sections = configuration.GetRequiredSection("Messaging").GetChildren();

        var configurations = new List<AzureServiceBusTransportConfiguration>();

        foreach (var section in sections)
        {
            if (section["Type"] != "AzureServiceBus")
            {
                continue;
            }

            var azureServiceBusTransportConfiguration = section.Get<AzureServiceBusTransportConfiguration>()!;

            configurations.Add(azureServiceBusTransportConfiguration);
        }

        return configurations;
    }
}