
namespace BitzArt.DependencyInjection;

/// <summary>
/// Provides a transient service provider that delegates service resolution 
/// to an inner <see cref="IServiceProvider"/>.
/// </summary>
public class TransientServiceProvider(IServiceProvider innerServiceProvider) : IServiceProvider
{
    private readonly IServiceProvider _innerServiceProvider = innerServiceProvider;

    /// <summary>
    /// Resolves a service of the specified type using the inner service provider.
    /// </summary>
    /// <param name="serviceType">The type of service to resolve.</param>
    /// <returns>The requested service instance or null if not found.</returns>
    public object? GetService(Type serviceType)
    {
        return _innerServiceProvider.GetService(serviceType);
    }
}