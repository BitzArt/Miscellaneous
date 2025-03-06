using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.DependencyInjection;

/// <summary>
/// Provides a transient service provider for creating isolated instances of services that delegates service resolution 
/// to an inner <see cref="IServiceProvider"/>.
/// </summary>
public class TransientServiceProvider(IServiceProvider innerServiceProvider) : ITransientServiceProvider
{
    private readonly IServiceProvider _innerServiceProvider = innerServiceProvider;

    /// <inheritdoc/>
    public object? GetService(Type serviceType)
    {
        return _innerServiceProvider.GetService(serviceType);
    }

    /// <inheritdoc/>
    public T? GetService<T>()
    {
        return (T?)GetService(typeof(T));
    }

    /// <inheritdoc/>
    public object GetRequiredService(Type serviceType)
    {
        return _innerServiceProvider.GetRequiredService(serviceType);
    }

    /// <inheritdoc/>
    public T GetRequiredService<T>() where T : notnull
    {
        return (T)GetRequiredService(typeof(T));
    }
}