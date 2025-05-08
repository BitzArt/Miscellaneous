using Wolverine;
using Wolverine.RabbitMQ;

namespace BitzArt;

public static class WolverineOptionsExtensions
{
    public static void ConfigureRabbitMqTransport(
        this WolverineOptions options,
        RabbitMqTransportConfiguration transportConfiguration,
        Action<WolverineOptions, object> implementationConfiguration)
    {
        var rabbitMq = options.UseRabbitMq(cfg =>
            {
                cfg.HostName = transportConfiguration.Host!;
                cfg.UserName = transportConfiguration.Username!;
                cfg.Password = transportConfiguration.Password!;
            })

            // Let Wolverine try to initialize any missing queues
            // on the first usage at runtime
            .AutoProvision()

            // Use Rabbit MQ for inter-node communication
            .EnableWolverineControlQueues();

        implementationConfiguration.Invoke(options, rabbitMq);
    }
}