using System.Collections.Immutable;
using System.Reflection;
using BitzArt.Extensions;
using BitzArt.Wolverine.Extensions.AzureServiceBus;
using BitzArt.Wolverine.Extensions.RabbitMq;
using BitzArt.Wolverine.Extensions.Sample.Common;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMessaging(configure: messaging =>
    {
        // Just for demonstration purposes - how you can use Wolverine's options
        messaging.WolverineOptions.UseNewtonsoftForSerialization();

        messaging
            .AddBus("bus-1", bus =>
            {
                bus
                        
                    .Topic("some-topic")
                    .ToQueue("some-queue-1")
                    .ToQueue("some-queue-2")
                    
                    .Topic("some-topic-2")
                    .ToQueue("some-queue-3");

                bus.ConfigureRabbitMq(builder.Configuration, messaging.WolverineOptions, rabbit =>
                {
                    //
                });

                bus.ConfigureAzureServiceBus(builder.Configuration, messaging.WolverineOptions, azure =>
                {
                    //
                });
            });
    },
    assemblies: ImmutableList<Assembly>.Empty);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/message-1", async (string value, IMessageBus messageBus) =>
{
    var message = new MyMessage
    {
        Value = value
    };

    await messageBus.PublishAsync(message);

    return $"{message} sent";
});

app.MapGet("/message-2", () =>
{
    
});

app.Run();
