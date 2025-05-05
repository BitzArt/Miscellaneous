using MediaMars.Messaging;

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
    Topic Topic(string name);
}

/// <summary>
/// Represents the configuration for a bus in the messaging system.
/// </summary>
public class BusConfiguration : IBusConfiguration
{
    private readonly List<Topic> _topics = new();

    /// <summary>
    /// Adds a topic to the bus configuration.
    /// </summary>
    /// <param name="topic">The topic to be added to the bus configuration.</param>
    public void Add(Topic topic)
    {
        _topics.Add(topic);
    }

    /// <inheritdoc />
    public Topic Topic(string name)
    {
        var topic = new Topic
        {
            Name = name
        };

        _topics.Add(topic);

        return topic;
    }
}