using System.Collections.Immutable;
using System.Reflection;
using BitzArt.Extensions;
using BitzArt.Wolverine.Extensions.AzureServiceBus;
using BitzArt.Wolverine.Extensions.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMessaging(
    configuration: builder.Configuration,
    configure: messaging =>
    {
        // Just for demonstration purposes - how you can use Wolverine's options
        messaging.WolverineOptions.UseNewtonsoftForSerialization();
        
        messaging.AddBus("my-bus-1", bus =>
        {
            bus
                .Topic("some-topic")
                    .ToQueue("some-queue-1")
                    .ToQueue("some-queue-2")
                .Topic("some-topic-2")
                    .ToQueue("some-queue-3");
            
            bus.ConfigureRabbitMq(rabbit =>
            {
                
            });

            bus.ConfigureAzureServiceBus(azure =>
            {
                
            });
        });
    },
    assemblies: ImmutableList<Assembly>.Empty);


var app = builder.Build();

app.MapGet("/", () => "Hello World!");


app.Run();