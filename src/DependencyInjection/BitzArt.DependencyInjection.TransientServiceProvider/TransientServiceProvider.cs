using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Reflection;

namespace BitzArt.DependencyInjection;

/// <summary>
/// The default implementation of <see cref="ITransientServiceProvider"/>.
/// Delegates service resolution to inner <see cref="IServiceProvider"/>.
/// </summary>
internal class TransientServiceProvider : ITransientServiceProvider
{
    private readonly IServiceProvider _globalServiceProvider;
    private readonly TransientServiceProviderOptions _options;
    private readonly IServiceProvider _innerServiceProvider;
    
    public TransientServiceProvider(IServiceProvider globalServiceProvider, 
        Action<IServiceCollection, IServiceProvider> configureServices, 
        Action<ITransientServiceProvider>? configure,
        TransientServiceProviderOptions options)
    {
        _globalServiceProvider = globalServiceProvider;
        _options = options;
        var sc = new ServiceCollection();
        configureServices(sc, globalServiceProvider);

        _innerServiceProvider = sc.BuildServiceProvider();
        configure?.Invoke(this);
    }

    /// <inheritdoc/>
    object? IServiceProvider.GetService(Type serviceType)
    {
        if (IsTypeEnumerable(serviceType) 
            || serviceType.GetInterfaces().Any(type => IsTypeEnumerable(type))) 
        {
            return GetEnumerable(serviceType);
        }
       
        var service = _innerServiceProvider.GetService(serviceType); 

        if (service is null && _options.FallbackToGlobal) {
            return _globalServiceProvider.GetService(serviceType);
        }

        return service;
    }

    private bool IsTypeEnumerable(Type serviceType)
    {        
        if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            return true;
        }

        return false;
    }

    private object? GetEnumerable(Type serviceType) 
    {
        var service = _innerServiceProvider.GetService(serviceType);

        if ((service as IEnumerable)!.GetEnumerator().MoveNext() is false
            && _options.FallbackToGlobal)
        {
            return _globalServiceProvider.GetService(serviceType);
        }

        return service;
    }
}