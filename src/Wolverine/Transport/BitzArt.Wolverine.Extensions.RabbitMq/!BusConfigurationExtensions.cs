using Microsoft.Extensions.Configuration;
using Wolverine;
using Wolverine.RabbitMQ;
using Wolverine.RabbitMQ.Internal;

namespace BitzArt.Wolverine.Extensions.RabbitMq;

public static class BusConfigurationExtensions
{
    public static IBusConfiguration ConfigureRabbitMq(
        this IBusConfiguration bus,
        IConfiguration configuration,
        WolverineOptions wolverineOptions,
        Action<RabbitMqTransportExpression>? configure = null)
    {
        var rabbitMqOptions = GetRabbitMqOptions(bus.Name, configuration);

        var rabbitMq = wolverineOptions.UseRabbitMq(cfg =>
            {
                cfg.HostName = rabbitMqOptions.Host!;
                cfg.UserName = rabbitMqOptions.Username!;
                cfg.Password = rabbitMqOptions.Password!;
            })

            // Let Wolverine try to initialize any missing queues
            // on the first usage at runtime
            .AutoProvision()

            // Use Rabbit MQ for inter-node communication
            .EnableWolverineControlQueues();

        configure?.Invoke(rabbitMq);
        
        return bus;
    }

    private static RabbitMqOptions GetRabbitMqOptions(string name, IConfiguration configuration)
    {
        var section = configuration
            .GetSection("Messaging")
            .GetChildren()
            .SingleOrDefault(section => section["Name"] == name && section["Type"] == RabbitMqOptions.BusType);

        if (section == null)
        {
            throw new InvalidOperationException($"No configuration found for bus '{name}'");
        }

        var rabbitMqOptions = new RabbitMqOptions
        {
            Name = section["Name"]!,
            Host = section["Host"]!,
            Password = section["Password"]!,
            Username = section["Username"]!,
            PrefetchCount = section.GetValue<int?>("PrefetchCount"),
        };
        return rabbitMqOptions;
    }
}