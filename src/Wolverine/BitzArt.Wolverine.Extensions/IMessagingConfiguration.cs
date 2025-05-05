using Wolverine;

namespace BitzArt;

/// <summary>
/// Defines an interface for configuring messaging infrastructure.
/// Facilitates adjustment of messaging settings and integration
/// with the Wolverine messaging framework.
/// </summary>
public interface IMessagingConfiguration
{
    /// <summary>
    /// Gets the WolverineOptions instance, which provides access to configuration
    /// settings and extensibility for the Wolverine messaging system.
    /// </summary>
    public WolverineOptions WolverineOptions { get; }
}

public class MessagingConfiguration : IMessagingConfiguration
{
    /// <inheritdoc />
    public required WolverineOptions WolverineOptions { get; init; }
}