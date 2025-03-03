using System.Collections.Concurrent;

namespace BitzArt.DependencyInjection;

/// <summary>
/// Factory for creating and managing transient service providers.
/// </summary>
/// <param name="serviceProvider">The root service provider.</param>
/// <param name="build">A function to create a new <see cref="TransientServiceProvider"/>.</param>
public class TransientServiceProviderFactory(
    IServiceProvider serviceProvider,
    Func<IServiceProvider, TransientServiceProvider> build)
{
    private readonly ConcurrentDictionary<string, TransientServiceProvider> _namedProviders = [];

    private readonly Lock _lock = new();

    private readonly Func<IServiceProvider, TransientServiceProvider> _build = build;

    /// <summary>
    /// Returns a transient service provider associated with the given name,
    /// creating and caching a new instance if not already present.
    /// </summary>
    /// <param name="name">The identifier for the provider instance.</param>
    /// <returns>A <see cref="TransientServiceProvider"/> instance.</returns>
    public TransientServiceProvider GetProvider(string name)
    {
        TransientServiceProvider? provider;

        if (!_namedProviders.TryGetValue(name, out provider))
        {
            lock (_lock)
            {
                if (!_namedProviders.TryGetValue(name, out provider))
                {
                    provider = _build(serviceProvider);
                    _namedProviders[name] = provider;
                }
            }
        }

        return provider;
    }

    /// <summary>
    /// Creates and returns a new transient service provider instance.
    /// </summary>
    /// <returns>A new <see cref="TransientServiceProvider"/> instance.</returns>
    public TransientServiceProvider GetProvider() => _build(serviceProvider);
}