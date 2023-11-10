using BitzArt.Probes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BitzArt;

public static class AddServiceInfoOptionsExtension
{
    public static IHostApplicationBuilder AddServiceInfoOptions(this IHostApplicationBuilder builder)
    {
        builder.Services.AddServiceInfoOptions(builder.Configuration);
        return builder;
    }

    public static IServiceCollection AddServiceInfoOptions(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration
            .GetRequiredSection(ServiceInfoOptions.SectionName)
            .Get<ServiceInfoOptions>()!;

        services.AddSingleton(options);

        return services;
    }
}
