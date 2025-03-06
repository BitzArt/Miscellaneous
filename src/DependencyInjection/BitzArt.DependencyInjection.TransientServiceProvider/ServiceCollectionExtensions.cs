using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.DependencyInjection;

/// <summary>
/// Provides DI container extensions.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the factory as a singleton and adds a transient registration that retrieves a provider from the factory.
    /// </summary>
    public static IServiceCollection AddTransientServiceProvider(
        this IServiceCollection services,
        Func<IServiceProvider, ITransientServiceProvider> build)
    {
        services.AddSingleton(sp => new TransientServiceProviderFactory(sp, build));

        services.AddTransient(x =>
        {
            var factory = x.GetRequiredService<TransientServiceProviderFactory>();
            var provider = factory.GetProvider();
            return provider;
        });

        return services;
    }
}