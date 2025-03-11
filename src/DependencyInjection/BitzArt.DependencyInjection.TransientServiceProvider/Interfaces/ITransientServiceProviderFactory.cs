namespace BitzArt.DependencyInjection;

/// <summary>
/// Provides methods to create transient service providers, or retrieve existing ones.
/// </summary>
public interface ITransientServiceProviderFactory
{
    /// <summary>
    /// Creates and returns a new transient service provider instance.
    /// </summary>
    /// <returns>A new <see cref="ITransientServiceProvider"/> instance.</returns>
    ITransientServiceProvider GetProvider();

    /// <summary>
    /// Returns a transient service provider associated with the given name,
    /// creating and caching a new instance if not already present.
    /// </summary>
    /// <param name="name">The name of the provider instance.</param>
    /// <returns>A <see cref="ITransientServiceProvider"/> instance.</returns>
    ITransientServiceProvider GetProvider(string name);
}