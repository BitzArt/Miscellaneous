using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace BitzArt.DependencyInjection;

internal class TransientServiceProviderFactory(
    IServiceProvider globalServiceProvider,
    Action<IServiceCollection, IServiceProvider> configureServices,
    Action<ITransientServiceProvider>? configure = null) : ITransientServiceProviderFactory
{
    private readonly IServiceProvider _globalServiceProvider = globalServiceProvider;

    private readonly ConcurrentDictionary<string, ITransientServiceProvider> _namedProviders = [];

    private readonly Lock _lock = new();

    private readonly Action<IServiceCollection, IServiceProvider> _configureServices = configureServices;

    private readonly Action<ITransientServiceProvider>? _configure = configure;

    public ITransientServiceProvider GetProvider(string name)
    {
        if (_namedProviders.TryGetValue(name, out var provider)) return provider;

        lock (_lock)
        {
            if (_namedProviders.TryGetValue(name, out provider)) return provider;

            provider = BuildServiceProvider();
            _namedProviders[name] = provider;

            return provider;
        }
    }

    public ITransientServiceProvider GetProvider() => BuildServiceProvider();

    private TransientServiceProvider BuildServiceProvider()
    {
        var sc = new ServiceCollection();
        _configureServices(sc, _globalServiceProvider);

        var provider = sc.BuildServiceProvider();

        var result = new TransientServiceProvider(provider);
        _configure?.Invoke(result);

        return result;
    }
}