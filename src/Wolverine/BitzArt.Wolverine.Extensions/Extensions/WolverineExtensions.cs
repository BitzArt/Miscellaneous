using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wolverine;

namespace BitzArt.Extensions;

/// <summary>
/// Extensions for adding messaging capabilities to the host application.
/// </summary>
public static class WolverineExtensions
{
    /// <summary>
    /// Adds messaging capabilities to the host application builder.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configure"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddMessaging(
        this IHostApplicationBuilder builder,
        Action<MessagingConfiguration> configure,
        IEnumerable<Assembly>? assemblies = null)
    {
        builder.Services.AddMessaging(configure,
            assemblies);

        return builder;
    }

    /// <summary>
    /// Adds messaging capabilities to the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configure"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IServiceCollection AddMessaging(
        this IServiceCollection services,
        Action<MessagingConfiguration> configure,
        IEnumerable<Assembly>? assemblies = null)
    {
        services.AddWolverine(wolverineOptions =>
        {
            // Opt out of Wolverine's default convention of routing messages to the local node's queues
            // Force messages without explicit routing rules to be sent to external transports even if
            // the node has a message handler for the message type
            wolverineOptions.Policies.DisableConventionalLocalRouting();

            if (assemblies != null)
            {
                foreach (var assembly in assemblies)
                {
                    wolverineOptions.Discovery.IncludeAssembly(assembly);
                }
            }

            var messagingConfiguration = new MessagingConfiguration
            {
                WolverineOptions = wolverineOptions
            };

            configure(messagingConfiguration);

            // var buses = messagingConfiguration.GetBuses();
            //
            // foreach (var bus in buses)
            // {
            //     // bus.
            // }
        });


        // throw new NotImplementedException();

        return services;
    }
}