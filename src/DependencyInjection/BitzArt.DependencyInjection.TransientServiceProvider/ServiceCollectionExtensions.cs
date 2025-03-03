using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTransientServiceProvider(
        this IServiceCollection services,
        Func<IServiceProvider, TransientServiceProvider> build)
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