using BitzArt;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting;

/// <summary>
/// Extension methods for <see cref="IHostBuilder"/>
/// </summary>
public static class HostBuilderExtensions
{
    /// <summary>
    /// Configures the host to use a <see cref="TransientServiceScopeFactory"/> as its <see cref="IServiceScopeFactory"/>.
    /// </summary>
    /// <param name="hostBuilder">An <see cref="IHostBuilder"/> instance to configure.</param>
    /// <returns>The same <see cref="IHostBuilder"/> instance to allow chaining.</returns>
    public static IHostBuilder UseTransientServiceScopeFactory(this IHostBuilder hostBuilder)
        => hostBuilder.UseServiceProviderFactory(new TransientServiceScopeServiceProviderFactory());
}
