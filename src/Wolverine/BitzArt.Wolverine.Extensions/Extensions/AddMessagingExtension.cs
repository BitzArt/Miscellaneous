using System.Reflection;
using BitzArt.TypeInfoResolvers;
using JasperFx.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wolverine;

namespace BitzArt.Messages;

/// <summary>
/// Adds Messaging to the Host Application Builder.
/// </summary>
public static class AddMessagingExtension
{
    /// <inheritdoc cref="AddMessaging(IServiceCollection, IConfiguration, Action{IBusConfiguration}, IEnumerable{Assembly}?)"/>
    /// <param name="builder"> The host application builder to configure messaging for.</param>
    /// <param name="configure">Bus configuration action.</param>
    /// <param name="assemblies">An optional function to specify assemblies to register for message handling.</param>
    public static IHostApplicationBuilder AddMessaging(
        this IHostApplicationBuilder builder,
        Action<IBusConfiguration> configure,
        IEnumerable<Assembly>? assemblies = null)
    {
        builder.Services.AddMessaging(
            builder.Configuration,
            configure,
            assemblies);

        return builder;
    }

    /// <summary>
    /// Adds Messaging to the Host Application Builder.
    /// </summary>
    /// <param name="services"> The service collection to configure messaging for.</param>
    /// <param name="configuration"> The configuration to read messaging options from.</param>
    /// <param name="configure">Bus configuration action.</param>
    /// <param name="assemblies">An optional function to specify assemblies to register for message handling.</param>
    /// <returns>The updated host application builder with messaging configured.</returns>
    public static IServiceCollection AddMessaging(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IBusConfiguration> configure,
        IEnumerable<Assembly>? assemblies = null)
    {
        var busConfiguration = new BusConfiguration();
        configure?.Invoke(busConfiguration);

        var busType = GetButType(configuration);

        services.AddWolverine(options =>
        {
            options.ConfigureJsonSerializerOptions();

            // Opt out of Wolverine's default convention of routing messages to the local node's queues
            // Force messages without explicit routing rules to be sent to external transports even if
            // the node has a message handler for the message type
            options.Policies.DisableConventionalLocalRouting();

            busConfiguration.CommonConfiguration?.Invoke(options);

            assemblies ??= GetAssembliesWhichContainsHandlers();

            foreach (var assembly in assemblies)
            {
                options.Discovery.IncludeAssembly(assembly);
            }

            var implementationConfiguration = busConfiguration.ImplementationConfiguration[busType];

            switch (busType)
            {
                case BusType.AzureServiceBus:
                    options.ConfigureAzureServiceBusTransport(configuration, implementationConfiguration);
                    break;

                case BusType.RabbitMQ:
                    options.ConfigureRabbitMqTransport(configuration, implementationConfiguration);
                    break;
            }
        });

        return services;
    }

    private static BusType GetButType(IConfiguration configuration)
    {
        var busType = configuration.GetRequiredSection("Messaging")
            .GetChildren()
            .Select(o => o["BusType"]!)
            .First()
            .ToEnum<BusType>();

        return busType;
    }

    /// <summary>
    /// Retrieves assemblies from the current application domain that contain types implementing
    /// specific interfaces, such as IConsumer<> or IRequestClient<,>.
    /// </summary>
    /// <returns>A collection of assemblies that include eligible handler types.</returns>
    private static IEnumerable<Assembly> GetAssembliesWhichContainsHandlers()
    {
        var appDomainAssemblies = AppDomain.CurrentDomain.GetAssemblies();

        var result = appDomainAssemblies.Where(assembly =>
            {
                try
                {
                    return assembly.GetTypes().Any(type =>
                        !type.IsAbstract &&
                        type.GetInterfaces().Any(i =>
                            i.IsGenericType &&
                            i.GetGenericTypeDefinition() == typeof(HandlerBase<>)));
                }
                catch (ReflectionTypeLoadException)
                {
                    // Skip assemblies that can't be loaded
                    return false;
                }
            })
            .ToList();

        return result;
    }

    private static void ConfigureJsonSerializerOptions(this WolverineOptions cfg)
    {
        cfg.UseSystemTextJsonForSerialization(options =>
        {
            options.TypeInfoResolver = new MessagingJsonTypeInfoResolver();
        });
    }
}