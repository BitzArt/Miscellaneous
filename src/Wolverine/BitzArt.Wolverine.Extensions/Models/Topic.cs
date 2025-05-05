namespace BitzArt;

/// <summary>
/// Represents a topic in the messaging system.
/// </summary>
public sealed record Topic
{
    private readonly List<Queue> _queues = new();

    /// <summary>
    /// The name of the topic.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Binds a queue to the topic.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Queue ToQueue(string name)
    {
        var queue = new Queue
        {
            Name = name
        };

        _queues.Add(queue);

        return queue;
    }
}