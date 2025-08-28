using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BitzArt.DependencyInjection;

/// <summary>
/// Extensions methods for <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <inheritdoc/>
    public static IServiceCollection AddTransientServiceProvider(
        this IServiceCollection services,
        Action<IServiceCollection> configureServices,
        Action<ITransientServiceProvider>? configureProvider = null,
        Action<TransientServiceProviderOptions>? configureOptions = null)
        => services.AddTransientServiceProvider(
            (serviceCollection, _) => configureServices(serviceCollection),
            configureProvider,
            configureOptions);

    /// <summary>
    /// Registers <see cref="ITransientServiceProviderFactory"/> and <see cref="ITransientServiceProvider"/> in the service collection.
    /// </summary>
    public static IServiceCollection AddTransientServiceProvider(
        this IServiceCollection services,
        Action<IServiceCollection, IServiceProvider> configureServices,
        Action<ITransientServiceProvider>? configureProvider = null,
        Action<TransientServiceProviderOptions>? configureOptions = null)
    {
        if (services.Any(x => x.ServiceType == typeof(ITransientServiceProviderFactory)))
        {
            throw new InvalidOperationException("ITransientServiceProviderFactory is already registered.");
        }

        services.AddSingleton<ITransientServiceProviderFactory>(globalServiceProvider =>
            new TransientServiceProviderFactory(globalServiceProvider, configureServices, configureProvider, configureOptions));

        services.AddTransient<ITransientServiceProvider>(x =>
        {
            var factory = x.GetRequiredService<ITransientServiceProviderFactory>();
            var provider = factory.GetProvider() as TransientServiceProvider;

            return provider!;
        });

        return services;
    }
}