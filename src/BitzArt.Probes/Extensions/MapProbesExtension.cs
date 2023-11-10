using BitzArt.Probes;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace BitzArt;

public static class MapProbesExtension
{
    public static IEndpointRouteBuilder MapRoutes(this IEndpointRouteBuilder builder)
    {
        builder.MapHealthChecks("_health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        var infoOptions = builder.ServiceProvider.GetService<ServiceInfoOptions>();

        if (infoOptions is null)
        {
            builder.MapGet("/", () => "It's good to be alive.");
        }
        else
        {
            var info = new ServiceInfoResponse(infoOptions);
            builder.MapGet("/", () => info);
        }

        return builder;
    }
}
