using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BitzArt.Extensions;

/// <summary>
/// Extensions for adding messaging capabilities to the host application.
/// </summary>
public static class WolverineExtensions
{
    /// <summary>
    /// Adds messaging capabilities to the host application builder.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddMessaging(
        this IHostApplicationBuilder builder,
        IEnumerable<Assembly>? assemblies = null)
    {
        builder.Services.AddMessaging(
            builder.Configuration,
            assemblies);

        return builder;
    }

    /// <summary>
    /// Adds messaging capabilities to the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IServiceCollection AddMessaging(
        this IServiceCollection services,
        IConfiguration configuration,
        IEnumerable<Assembly>? assemblies = null)
    {
        throw new NotImplementedException();
    }
}