using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BitzArt.ConsoleTools;

public static class AddConsoleToolExtensions
{
    public static IServiceCollection AddConsoleToolsFromAssemblyContaining<TAssemblyPointer>(this IServiceCollection services)
        => services.AddConsoleToolsFromAssemblyContaining(typeof(TAssemblyPointer));

    public static IServiceCollection AddConsoleToolsFromAssemblyContaining(this IServiceCollection services, Type type)
        => services.AddConsoleToolsFromAssembly(type.Assembly);

    public static IServiceCollection AddConsoleToolsFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var tools = assembly
            .DefinedTypes
            .Where(x => x.IsAbstract == false)
            .Where(x => x.GetInterfaces().Contains(typeof(IConsoleTool)));

        foreach (var tool in tools) services.AddConsoleTool(tool);

        return services;
    }

    public static IServiceCollection AddConsoleTool<TConsoleTool>(this IServiceCollection services)
        where TConsoleTool : class, IConsoleTool
    {
        services.AddTransient<TConsoleTool>();
        services.AddTransient<IConsoleTool, TConsoleTool>();

        return services;
    }

    public static IServiceCollection AddConsoleTool(this IServiceCollection services, Type type)
    {
        if (type is null) throw new ArgumentException($"{nameof(type)} must not be null");
        if (type.IsAssignableTo(typeof(IConsoleTool)) == false) throw new ArgumentException($"{type.Name} is not assignable to IConsoleTool");

        services.AddTransient(type);
        services.AddTransient(x => (IConsoleTool)x.GetRequiredService(type));

        return services;
    }
}
