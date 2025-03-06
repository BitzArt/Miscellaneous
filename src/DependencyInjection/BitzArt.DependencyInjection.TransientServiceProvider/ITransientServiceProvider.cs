namespace BitzArt.DependencyInjection;

/// <summary>
/// Provides a transient service provider for creating isolated instances of services.
/// </summary>
public interface ITransientServiceProvider : IServiceProvider
{
    /// <summary>
    /// Get service of type serviceType from the inner <see cref="IServiceProvider"/>.
    /// </summary>
    /// <param name="serviceType">The type of service to resolve.</param>
    /// <returns>The requested service instance or null if not found.</returns>
    public new object? GetService(Type serviceType);

    /// <summary>
    /// Get service of type <typeparamref name="T"/> from the inner <see cref="IServiceProvider"/>.
    /// </summary>
    /// <typeparam name="T">The type of service object to get.</typeparam>
    /// <returns>A service object of type <typeparamref name="T"/> or null if there is no such service.</returns>
    public T? GetService<T>();

    /// <summary>
    /// Get service of type <paramref name="serviceType"/> from the inner <see cref="IServiceProvider"/>.
    /// </summary>
    /// <param name="serviceType">An object that specifies the type of service object to get.</param>
    /// <returns>A service object of type <paramref name="serviceType"/>.</returns>
    public object GetRequiredService(Type serviceType);

    /// <summary>
    /// Get service of type <typeparamref name="T"/> from the inner <see cref="IServiceProvider"/>.
    /// </summary>
    /// <typeparam name="T">The type of service object to get.</typeparam>
    /// <returns>A service object of type <typeparamref name="T"/>.</returns>
    public T GetRequiredService<T>() where T : notnull;
}