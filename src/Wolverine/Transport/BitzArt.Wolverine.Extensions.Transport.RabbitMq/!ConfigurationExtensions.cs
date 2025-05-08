using Microsoft.Extensions.Configuration;

namespace BitzArt;

public static class ConfigurationExtensions
{
    public static IReadOnlyCollection<RabbitMqTransportConfiguration> GetRabbitMqTransportConfigurations(
        this IConfiguration configuration)
    {
        var sections = configuration.GetRequiredSection("Messaging").GetChildren();

        var configurations = new List<RabbitMqTransportConfiguration>();
        
        foreach (var section in sections)
        {
            if (section["Type"] != "RabbitMQ")
            {
                continue;
            }
            
            configurations.Add(new RabbitMqTransportConfiguration
            {
                Host = section["Host"],
                Username = section["Username"],
                Password = section["Password"],
                PrefetchCount = section.GetValue<int?>("PrefetchCount"),
                Name = section["Name"]
            });
        }

        return configurations;
    }
}