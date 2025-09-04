using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.XUnit;

internal class ServiceProviderFactory : IServiceProviderFactory<ServiceProviderFactory>
{
    private IServiceCollection? _services;

    public ServiceProviderFactory CreateBuilder(IServiceCollection services)
    {
        _services = services;
        return this;
    }

    public IServiceProvider CreateServiceProvider(ServiceProviderFactory containerBuilder)
    {
        return new TransientScopeBuilder(_services!.BuildServiceProvider());
    }
}

internal class TransientScopeBuilder(IServiceProvider globalServiceProvider) : IServiceProvider
{
    public object? GetService(Type serviceType)
    {
        if (serviceType == typeof(IServiceScopeFactory))
        {
            return new TransientScopeFactory(globalServiceProvider);
        }

        return globalServiceProvider.GetService(serviceType);
    }
}

internal class TransientScopeFactory(IServiceProvider globalServiceProvider) : IServiceScopeFactory
{
    public IServiceScope CreateScope()
    {
        return new TransientScope(globalServiceProvider);
    }
}

internal class TransientScope(IServiceProvider globalServiceProvider) : IServiceScope
{
    private readonly IServiceScope _internalScope = globalServiceProvider.CreateScope();

    public IServiceProvider ServiceProvider
        => _internalScope.ServiceProvider.GetRequiredService<ITransientServiceProvider>();

    public void Dispose() => _internalScope.Dispose();
}
