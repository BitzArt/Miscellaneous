using System.Collections.Concurrent;

namespace BitzArt;

public class TransientServiceProviderFactory(
    IServiceProvider serviceProvider,
    Func<IServiceProvider, TransientServiceProvider> build)
{
    private readonly ConcurrentDictionary<string, TransientServiceProvider> _namedProviders = [];

    private readonly Lock _lock = new();

    private readonly Func<IServiceProvider, TransientServiceProvider> _build = build;

    public TransientServiceProvider GetProvider(string name)
    {
        lock (_lock)
        {
            if (!_namedProviders.TryGetValue(name, out var provider))
            {
                provider = _build(serviceProvider);
                _namedProviders[name] = provider;
            }

            return provider;
        }
    }

    public TransientServiceProvider GetProvider() => _build(serviceProvider);
}