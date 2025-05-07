using Wolverine;

namespace BitzArt;

public class MessagingConfiguration
{
    private readonly List<IBusConfiguration> _buses = new();

    /// <summary>
    /// The options for configuring the messaging system.
    /// </summary>
    public required WolverineOptions WolverineOptions { get; init; }

    /// <summary>
    /// Adds a new bus to the messaging configuration with the specified name and configuration.
    /// </summary>
    /// <param name="name">The unique name of the bus to be added.</param>
    /// <param name="configuration">An action to configure the specific bus settings.</param>
    /// <returns>The updated messaging configuration with the newly added bus.</returns>
    public MessagingConfiguration AddBus(string name, Action<IBusConfiguration> configuration)
    {
        var busConfiguration = new BusConfiguration
        {
            Name = name,
        };
        
        _buses.Add(busConfiguration);

        configuration(busConfiguration);

        return this;
    }

    public IReadOnlyCollection<IBusConfiguration> GetBuses() => _buses;
}