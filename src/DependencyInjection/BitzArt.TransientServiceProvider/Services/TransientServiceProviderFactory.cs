using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace BitzArt;

internal class TransientServiceProviderFactory : ITransientServiceProviderFactory
{
    private readonly IServiceProvider _globalServiceProvider;
    private readonly ConcurrentDictionary<string, ITransientServiceProvider> _namedProviders;
    private readonly Lock _lock;
    private readonly Action<IServiceCollection, IServiceProvider> _configureServices;
    private readonly Action<ITransientServiceProvider>? _configure;
    private readonly TransientServiceProviderOptions _options;

    public TransientServiceProviderFactory(
        IServiceProvider globalServiceProvider,
        Action<IServiceCollection, IServiceProvider> configureServices,
        Action<ITransientServiceProvider>? configure = null,
        Action<TransientServiceProviderOptions>? configureOptions = null)
    {
        _globalServiceProvider = globalServiceProvider;
        _configureServices = configureServices;
        _configure = configure;

        _lock = new();
        _namedProviders = [];

        _options = new TransientServiceProviderOptions();
        configureOptions?.Invoke(_options);
    }

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

    private TransientServiceProvider BuildServiceProvider() => new(_globalServiceProvider, _configureServices, _configure, _options);

}