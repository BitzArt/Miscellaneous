using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.DependencyInjection;

/// <summary>
/// Extensions methods for <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="ITransientServiceProviderFactory"/> and <see cref="ITransientServiceProvider"/> in the service collection.
    /// </summary>
    public static IServiceCollection AddTransientServiceProvider(
        this IServiceCollection services,
        Action<IServiceCollection> configureServices,
        Action<ITransientServiceProvider>? configure = null)
    {
        services.AddSingleton<ITransientServiceProviderFactory>(sp =>
            new TransientServiceProviderFactory(configureServices, configure));

        services.AddTransient<ITransientServiceProvider>(x =>
        {
            var factory = x.GetRequiredService<ITransientServiceProviderFactory>();
            var provider = factory.GetProvider() as TransientServiceProvider;

            return provider!;
        });

        return services;
    }
}