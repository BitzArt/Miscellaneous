using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.DependencyInjection;

/// <summary>
/// Provides DI container extensions.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers an <see cref="TransientServiceProviderFactory"/>
    /// </summary>
    public static IServiceCollection AddTransientServiceProvider(
        this IServiceCollection services,
        Action<IServiceCollection> configureServices,
        Action<ITransientServiceProvider>? configure = null)
    {
        services.AddSingleton<ITransientServiceProviderFactory, TransientServiceProviderFactory>(sp =>
            new TransientServiceProviderFactory(configureServices, configure));

        services.AddTransient(x =>
        {
            var factory = x.GetRequiredService<ITransientServiceProviderFactory>();
            var provider = factory.GetProvider();
            return provider;
        });

        return services;
    }
}