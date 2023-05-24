using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace MassTransit;

public static class AddProcessorsExtension
{
    public static IServiceCollection AddProcessors<TProcessorsAssemblyPointer>(this IServiceCollection services)
    {
        var processorTypes = typeof(TProcessorsAssemblyPointer)
            .Assembly.DefinedTypes
            .Where(x => !x.IsAbstract)
            .Where(x => x.GetInterfaces().Any(xx =>
                xx.IsGenericType &&
                xx.GetGenericTypeDefinition() == typeof(IProcessor<>)));

        foreach (var type in processorTypes)
        {
            var interfaceType = type.GetInterfaces().First(x =>
                x.IsGenericType &&
                x.GetGenericTypeDefinition() == typeof(IProcessor<>));
            var messageType = interfaceType.GenericTypeArguments.First();
            services.AddScoped(type);
            services.AddScoped(interfaceType, x => x.GetRequiredService(type));
        }

        return services;
    }
}
