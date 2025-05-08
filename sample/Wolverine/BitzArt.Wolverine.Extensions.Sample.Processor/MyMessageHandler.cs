using BitzArt.Wolverine.Extensions.Sample.Common;

namespace BitzArt.Wolverine.Extensions.Sample.Processor;

public class MyMessageHandler
{
    public async Task HandleAsync(MyMessage message)
    {
        Console.WriteLine("Received message: " + message.Value);
    }
}