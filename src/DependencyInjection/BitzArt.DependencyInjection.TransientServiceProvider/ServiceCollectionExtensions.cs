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