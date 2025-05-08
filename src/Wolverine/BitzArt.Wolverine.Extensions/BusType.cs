using System.Runtime.Serialization;

namespace BitzArt;

/// <summary>
/// Messaging bus type.
/// </summary>
public enum BusType : byte
{
    /// <summary>
    /// Azure Service Bus.
    /// </summary>
    [EnumMember(Value = "AzureServiceBus")]
    AzureServiceBus = 1,

    /// <summary>
    /// RabbitMQ.
    /// </summary>
    [EnumMember(Value = "RabbitMQ")]
    RabbitMQ = 2,
    
    /// <summary>
    /// Kafka.
    /// </summary>
    [EnumMember(Value = "Kafka")]
    Kafka = 3,
}