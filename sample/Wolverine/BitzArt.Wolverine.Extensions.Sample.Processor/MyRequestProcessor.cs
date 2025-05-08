using BitzArt.ApiExceptions;
using BitzArt.Messages;
using BitzArt.Wolverine.Extensions.Sample.Common;
using Microsoft.Extensions.Logging.Abstractions;
using Wolverine;

namespace BitzArt.Wolverine.Extensions.Sample.Processor;

public class MyRequestProcessor : RequestProcessor<MyRequest>
{
    public MyRequestProcessor()
        : this(new NullLogger<MyMessageHandler>())
    {
    }

    public MyRequestProcessor(ILogger logger)
        : base(logger)
    {
    }

    protected override async Task<ResponseMessage> ProcessAsync(MyRequest message, IMessageContext context)
    {
        var response = new ResponseMessage<MyResponse>
        {
            Data = new MyResponse
            {
                Value = "I respond to your request: " + message.Value,
                Request = message
            },
            StatusCode = ApiStatusCode.OK
        };

        await Task.Delay(1000);

        return response;
    }
}