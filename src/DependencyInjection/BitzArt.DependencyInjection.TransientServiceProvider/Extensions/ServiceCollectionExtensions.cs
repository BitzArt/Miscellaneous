using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

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

        services.AddTransient<ITransientServiceProvider, TransientServiceProvider>(x =>
        {
            var factory = x.GetRequiredService<ITransientServiceProviderFactory>();
            var provider = factory.GetProvider() as TransientServiceProvider;

            if (provider is null)
                throw new UnreachableException("Unable to retrieve an instance of TransientServiceProvider from the factory.");

            return provider;
        });

        return services;
    }
}