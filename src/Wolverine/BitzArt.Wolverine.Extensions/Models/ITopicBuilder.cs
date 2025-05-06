namespace BitzArt;

public interface ITopicBuilder
{
    ITopicBuilder Topic(string topicName);
    ITopicBuilder ToQueue(string queueName);
}

public class TopicBuilder : ITopicBuilder
{
    private readonly string _name;
    private readonly List<Queue> _queues;
    private readonly List<ITopicBuilder> _topics;

    public TopicBuilder(string name)
    {
        _name = name;
        _queues = new List<Queue>();
        _topics = new List<ITopicBuilder>();
    }

    public ITopicBuilder Topic(string topicName)
    {
        var topicBuilder = new TopicBuilder(topicName);

        _topics.Add(topicBuilder);

        return topicBuilder;
    }

    public ITopicBuilder ToQueue(string queueName)
    {
        var queue = new Queue()
        {
            Name = queueName
        };

        _queues.Add(queue);

        return this;
    }

    public IReadOnlyDictionary<string, List<string>> Build()
    {
        var result = new Dictionary<string, List<string>>();

        if (_queues.Any())
        {
            result[_name] = _queues.Select(q => q.Name).ToList();
        }

        foreach (ITopicBuilder topic in _topics)
        {
            if (topic is TopicBuilder builder)
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