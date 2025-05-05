using MediaMars.Messaging;

namespace BitzArt.Extensions;

/// <summary>
/// Provides extension methods for configuring the messaging infrastructure.
/// </summary>
public static class MessagingConfigurationExtensions
{
    /// <summary>
    /// Adds a new bus to the messaging configuration with the specified name and configuration.
    /// </summary>
    /// <param name="messaging">The current messaging configuration.</param>
    /// <param name="name">The unique name of the bus to be added.</param>
    /// <param name="configuration">An action to configure the specific bus settings.</param>
    /// <returns>The updated messaging configuration with the newly added bus.</returns>
    public static IMessagingConfiguration AddBus(
        this IMessagingConfiguration messaging,
        string name,
        Action<IBusConfiguration> configuration)
    {
        var busConfiguration = new BusConfiguration();
        configuration(busConfiguration);
        
        //TODO: implement bus configuration
        
        return messaging;
    }
}