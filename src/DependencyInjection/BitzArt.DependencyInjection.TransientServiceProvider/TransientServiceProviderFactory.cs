using System.Collections.Concurrent;

namespace BitzArt.DependencyInjection;

/// <summary>
/// Provides thread-safe caching of named transient service providers,
/// while also enabling on-demand creation of uncached instances.
/// </summary>
/// <param name="serviceProvider">The root service provider.</param>
/// <param name="build">A function to create a new <see cref="ITransientServiceProvider"/>.</param>
public class TransientServiceProviderFactory(
    IServiceProvider serviceProvider,
    Func<IServiceProvider, ITransientServiceProvider> build)
{
    private readonly ConcurrentDictionary<string, ITransientServiceProvider> _namedProviders = [];

    private readonly Lock _lock = new();

    private readonly Func<IServiceProvider, ITransientServiceProvider> _build = build;

    /// <summary>
    /// Returns a transient service provider associated with the given name,
    /// creating and caching a new instance if not already present.
    /// </summary>
    /// <param name="name">The identifier for the provider instance.</param>
    /// <returns>A <see cref="ITransientServiceProvider"/> instance.</returns>
    public ITransientServiceProvider GetProvider(string name)
    {
        ITransientServiceProvider? provider;

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
    /// <returns>A new <see cref="ITransientServiceProvider"/> instance.</returns>
    public ITransientServiceProvider GetProvider() => _build(serviceProvider);
}