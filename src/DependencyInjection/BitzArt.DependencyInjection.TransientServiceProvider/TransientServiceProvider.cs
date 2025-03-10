using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.DependencyInjection;

/// <summary>
/// The default implementation of <see cref="ITransientServiceProvider"/>.
/// Delegates service resolution to inner <see cref="IServiceProvider"/>.
/// </summary>
internal class TransientServiceProvider(IServiceProvider innerServiceProvider) : ITransientServiceProvider
{
    private readonly IServiceProvider _innerServiceProvider = innerServiceProvider;

    /// <inheritdoc/>
    object? IServiceProvider.GetService(Type serviceType)
    {
        return _innerServiceProvider.GetService(serviceType);
    }
}