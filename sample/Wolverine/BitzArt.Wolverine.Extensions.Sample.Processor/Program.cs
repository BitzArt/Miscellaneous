using BitzArt;
using BitzArt.Messages;
using BitzArt.Wolverine.Extensions.Sample.Processor;
using Wolverine.AzureServiceBus;
using Wolverine.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<MyMessageHandler>(p =>
{
    return new MyMessageHandler(p.GetRequiredService<ILogger<MyMessageHandler>>());
});

builder.Services.AddSingleton<MyRequestProcessor>(p =>
{
    return new MyRequestProcessor(p.GetRequiredService<ILogger<MyRequestProcessor>>());
});

builder.Services.AddMessaging(
    configuration: builder.Configuration,
    configure: configuration =>
    {
        configuration.ConfigureRabbitMq((wolverine, rabbitMq) =>
        {
            rabbitMq.BindExchange("msg").ToQueue("messages");
            rabbitMq.BindExchange("ping-pong").ToQueue("pp");

            wolverine.ListenToRabbitQueue("messages");
            wolverine.ListenToRabbitQueue("pp");
        });

        configuration.ConfigureAzureServiceBus((wolverine, serviceBus) =>
        {
            wolverine.ListenToAzureServiceBusSubscription("messages").FromTopic("msg");
        });
    }, assemblies:
    [
        typeof(MyMessageHandler).Assembly
    ]);

var app = builder.Build();

app.Run();