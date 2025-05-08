using BitzArt.Messages;
using BitzArt.Wolverine.Extensions.Sample.Common;
using Microsoft.Extensions.Logging.Abstractions;
using Wolverine;

namespace BitzArt.Wolverine.Extensions.Sample.Processor;

public class MyMessageHandler : HandlerBase<MyMessage>
{
    public MyMessageHandler()
        : this(new NullLogger<MyMessageHandler>())
    {
    }

    public MyMessageHandler(ILogger logger)
        : base(logger)
    {
    }

    protected override Task ProcessAsync(MyMessage message, IMessageContext context)
    {
        Console.WriteLine($"Processing message: {message.Value}");

        return Task.Delay(1000);
    }
}