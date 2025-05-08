using BitzArt;
using BitzArt.Messages;
using BitzArt.Wolverine.Extensions.Sample.Processor;
using Wolverine.AzureServiceBus;
using Wolverine.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMessaging(
    configuration: builder.Configuration,
    configure: configuration =>
    {
        configuration.ConfigureRabbitMq((wolverine, rabbitMq) =>
        {
            rabbitMq.BindExchange("msg").ToQueue("messages");

            wolverine.ListenToRabbitQueue("messages");
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