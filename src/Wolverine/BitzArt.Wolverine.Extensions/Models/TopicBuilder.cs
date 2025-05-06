namespace BitzArt;

/// <summary>
/// Provides an interface for configuring and managing topics and their associated queues.
/// </summary>
public interface ITopicBuilder
{
    /// <summary>
    /// Adds a topic to the configuration.
    /// </summary>
    /// <param name="topicName">The name of the nested topic to be added.</param>
    /// <returns>A reference to the new <see cref="ITopicBuilder"/> instance created for the nested topic.</returns>
    ITopicBuilder Topic(string topicName);

    /// <summary>
    /// Bind a queue to the topic.
    /// </summary>
    /// <param name="queueName">The name of the queue to be added to the topic.</param>
    /// <returns>A reference to the current <see cref="ITopicBuilder"/> instance for method chaining.</returns>
    ITopicBuilder ToQueue(string queueName);
}

/// <summary>
/// 
/// </summary>
public class TopicBuilder : ITopicBuilder
{
    private readonly string _name;
    private readonly List<Queue> _queues;
    private readonly List<ITopicBuilder> _topics;

    /// <summary>
    /// Provides configuration capabilities for creating and managing topics and their associated message queues in the messaging system.
    /// </summary>
    public TopicBuilder(string name)
    {
        _name = name;
        _queues = new List<Queue>();
        _topics = new List<ITopicBuilder>();
    }

    /// <inheritdoc />
    public ITopicBuilder Topic(string topicName)
    {
        var topicBuilder = new TopicBuilder(topicName);

        _topics.Add(topicBuilder);

        return topicBuilder;
    }

    /// <inheritdoc />    
    public ITopicBuilder ToQueue(string queueName)
    {
        var queue = new Queue()
        {
            Name = queueName
        };

        _queues.Add(queue);

        return this;
    }

    /// <summary>
    /// Builds and returns the hierarchical mapping of topics and their associated queues.
    /// </summary>
    public IReadOnlyDictionary<string, List<string>> Build()
    {
        var result = new Dictionary<string, List<string>>();

        if (_queues.Any())
        {
            result[_name] = _queues.Select(q => q.Name).ToList();
        }

        foreach (var topicBuilder in _topics)
        {
            if (topicBuilder is TopicBuilder builder)
            {
                var childResult = builder.Build();

                foreach (var entry in childResult)
                {
                    if (result.ContainsKey(entry.Key))
                    {
                        result[entry.Key].AddRange(entry.Value);
                    }
                    else
                    {
                        result[entry.Key] = entry.Value;
                    }
                }
            }
        }

        return result;
    }
}