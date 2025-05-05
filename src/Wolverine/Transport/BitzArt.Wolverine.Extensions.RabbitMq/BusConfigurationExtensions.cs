using MediaMars.Messaging;
using Wolverine.RabbitMQ.Internal;

namespace BitzArt.Wolverine.Extensions.RabbitMq;

public static class BusConfigurationExtensions
{
    public static IBusConfiguration ConfigureRabbitMq(
        this IBusConfiguration configuration,
        Action<RabbitMqTransportExpression>? configure = null)
    {
        //TODO: implement RabbitMq configuration
        
        return configuration;
    }
}