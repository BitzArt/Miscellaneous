using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BitzArt.XUnit;

public class Startup
{
    public IHost BuildHost(IHostBuilder hostBuilder)
    {
        hostBuilder.UseServiceProviderFactory(new ServiceProviderFactory());
        return hostBuilder.Build();
    }

    private class ServiceProviderFactory : IServiceProviderFactory<ServiceProviderFactory>
    {
        private IServiceCollection _services = null!;

        public ServiceProviderFactory CreateBuilder(IServiceCollection services)
        {
            _services = services;
            return this;
        }

        public IServiceProvider CreateServiceProvider(ServiceProviderFactory containerBuilder)
        {
            return _services.BuildServiceProvider().GetRequiredService<ITransientServiceProvider>();
        }
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var globalId = Guid.NewGuid();

        services.AddTransientServiceProvider(t =>
        {
            t.AddTransient((_) => new TransientDependency(globalId));
        },
        configureOptions: options =>
        {
            options.FallbackToGlobal = true;
        });
    }
}
