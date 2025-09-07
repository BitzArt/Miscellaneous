using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BitzArt.XUnit;

public partial class Startup
{
    public IHost BuildHost(IHostBuilder hostBuilder)
    {
        hostBuilder.UseServiceProviderFactory(new ServiceProviderFactory());
        return hostBuilder.Build();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var globalId = Guid.NewGuid();

        services.AddTransientServiceProvider(t =>
        {
            var localId = Guid.NewGuid();

            t.AddSingleton(new TransientDependency(globalId, localId));
        },
        configureOptions: options =>
        {
            options.FallbackToGlobal = true;
        });
    }
}
