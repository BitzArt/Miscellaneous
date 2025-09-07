using Microsoft.Extensions.DependencyInjection;

namespace BitzArt;

internal class TransientServiceScopeServiceProviderFactory : IServiceProviderFactory<TransientServiceScopeServiceProviderFactory>
{
    private IServiceCollection? _services;

    public TransientServiceScopeServiceProviderFactory CreateBuilder(IServiceCollection services)
    {
        _services = services;
        return this;
    }

    public IServiceProvider CreateServiceProvider(TransientServiceScopeServiceProviderFactory containerBuilder)
    {
        return new TransientScopeBuilder(_services!.BuildServiceProvider());
    }

    private class TransientScopeBuilder(IServiceProvider globalServiceProvider) : IServiceProvider
    {
        public object? GetService(Type serviceType)
        {
            if (serviceType == typeof(IServiceScopeFactory))
            {
                return new TransientServiceScopeFactory(globalServiceProvider);
            }

            return globalServiceProvider.GetService(serviceType);
        }
    }
}
