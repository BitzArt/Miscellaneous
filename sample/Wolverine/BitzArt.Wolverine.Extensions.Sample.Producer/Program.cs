using BitzArt;
using BitzArt.Messages;
using BitzArt.Wolverine.Extensions.Sample.Common;
using Scalar.AspNetCore;
using Wolverine;
using Wolverine.AzureServiceBus;
using Wolverine.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMessaging(
    configuration: builder.Configuration,
    configure: configuration =>
    {
        configuration.ConfigureRabbitMq((wolverine, rabbitMq) =>
        {
            wolverine.PublishMessage<MyMessage>().ToRabbitExchange("msg");
            wolverine.PublishMessage<MyRequest>().ToRabbitExchange("ping-pong");
        });

        configuration.ConfigureAzureServiceBus((wolverine, serviceBus) =>
        {
            wolverine.PublishMessage<MyMessage>().ToAzureServiceBusTopic("msg");
            wolverine.PublishMessage<MyRequest>().ToAzureServiceBusTopic("ping-pong");
        });
    });

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();

app.MapScalarApiReference("/", options =>
{
    options.Theme = ScalarTheme.Mars;
    options.DarkMode = false;
});

app.MapGet("/message", (string value, IMessageBus bus) =>
{
    var message = new MyMessage
    {
        Value = value
    };
    
    bus.PublishAsync(message);
    
    Console.WriteLine("Message published");
});

app.MapGet("/ping", async (string value, IMessageBus bus) =>
{
    var request = new MyRequest
    {
        Value = value
    };

    var response = await bus.InvokeAsync<ResponseMessage<MyResponse>>(request);
    
    Console.WriteLine($"Response: {response.Data.Value}");
    
});

app.Run();