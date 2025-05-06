namespace BitzArt;

/// <summary>
/// Defines the contract for configuring a bus in the messaging system.
/// </summary>
public interface IBusConfiguration
{
    /// <summary>
    /// Adds a topic to the bus configuration.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    ITopicBuilder Topic(string name);
}

/// <summary>
/// Represents the configuration for a bus in the messaging system.
/// </summary>
public class BusConfiguration : IBusConfiguration
{
    private readonly ITopicBuilder _topicBuilder = new TopicBuilder("");
   

    /// <inheritdoc />
    public ITopicBuilder Topic(string name)
    {
        return _topicBuilder.Topic(name);
    }
}