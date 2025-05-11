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

            var rabbitMqTransportConfiguration = section.Get<RabbitMqTransportConfiguration>()!;

            configurations.Add(rabbitMqTransportConfiguration);
        }

        return configurations;
    }
}