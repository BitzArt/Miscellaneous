using BitzArt.Probes;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace BitzArt;

public static class MapProbesExtension
{
    public static IEndpointRouteBuilder MapProbes(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/", () => ProbeMessages.GetOne());
        MapHealthChecks(builder);
        MapServiceInfo(builder);

        return builder;
    }

    private static void MapHealthChecks(IEndpointRouteBuilder builder)
    {
        builder.MapHealthChecks("_health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    }

    private static void MapServiceInfo(IEndpointRouteBuilder builder)
    {
        var infoOptions = builder.ServiceProvider.GetService<ServiceInfoOptions>();
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var info = new ServiceInfoResponse(infoOptions, environment);

        builder.MapGet("_info", () => info);
    }
}
