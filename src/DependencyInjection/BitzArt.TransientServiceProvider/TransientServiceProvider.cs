using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BitzArt;

/// <summary>
/// The default implementation of <see cref="ITransientServiceProvider"/>.
/// Delegates service resolution to inner <see cref="IServiceProvider"/>.
/// </summary>
internal class TransientServiceProvider : ITransientServiceProvider
{
    private readonly IServiceProvider _globalServiceProvider;
    private readonly TransientServiceProviderOptions _options;

    private readonly ServiceProvider _innerServiceProvider;

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
        if (ImplementsEnumerable(serviceType, out var targetType))
        {
            var method = GetType().GetMethod(nameof(GetEnumerable), BindingFlags.NonPublic | BindingFlags.Instance);
            var genericMethod = method?.MakeGenericMethod(targetType!);
            return genericMethod!.Invoke(this, [serviceType]);
        }

        var service = _innerServiceProvider.GetService(serviceType);

        if (service is null && _options.FallbackToGlobal)
        {
            return _globalServiceProvider.GetService(serviceType);
        }

        return service;
    }

    private object? GetEnumerable<T>(Type serviceType)
    {
        var service = (IEnumerable<T>)_innerServiceProvider.GetService(serviceType)!;

        if (!service.Any() && _options.FallbackToGlobal)
        {
            return _globalServiceProvider.GetService(serviceType);
        }

        return service;
    }

    private static bool ImplementsEnumerable(Type serviceType, out Type? targetType)
    {
        // Check if the requested type itself is IEnumerable<T>
        if (IsEnumerable(serviceType, out targetType))
        {
            return true;
        }

        // Check for IEnumerable<T> in type's implemented interfaces
        var implementedInterfaces = serviceType.GetInterfaces();

        foreach (var implementedInterface in implementedInterfaces)
        {
            if (IsEnumerable(implementedInterface, out targetType))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsEnumerable(Type serviceType, out Type? targetType)
    {
        targetType = null;

        if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            targetType = serviceType.GetGenericArguments()[0];
            return true;
        }

        return false;
    }
}