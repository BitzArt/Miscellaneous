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
            if (section["Type"] != "RabbitMQ")
            {
                continue;
            }
            
            configurations.Add(new AzureServiceBusTransportConfiguration
            {
                ConnectionString = section["ConnectionString"]!,
                PrefetchCount = section.GetValue<int?>("PrefetchCount"),
                Name = section["Name"]
            });
        }

        return configurations;
    }
}