using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BitzArt.Console;

public static class AddConsoleMenuExtensions
{
    public static IServiceCollection AddConsoleMenusFromAssemblyContaining<TAssemblyPointer>(this IServiceCollection services)
        => services.AddConsoleMenusFromAssemblyContaining(typeof(TAssemblyPointer));

    public static IServiceCollection AddConsoleMenusFromAssemblyContaining(this IServiceCollection services, Type type)
        => services.AddConsoleMenusFromAssembly(type.Assembly);

    public static IServiceCollection AddConsoleMenusFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var tools = assembly
            .DefinedTypes
            .Where(x => x.IsAbstract == false)
            .Where(x => x.GetInterfaces().Contains(typeof(IConsoleMenu)));

        foreach (var tool in tools) services.AddConsoleMenu(tool);

        return services;
    }

    public static IServiceCollection AddConsoleMenu<TConsoleTool>(this IServiceCollection services)
        where TConsoleTool : class, IConsoleMenu
    {
        services.AddTransient<TConsoleTool>();
        services.AddTransient<IConsoleMenu, TConsoleTool>();

        return services;
    }

    public static IServiceCollection AddConsoleMenu(this IServiceCollection services, Type type)
    {
        if (type is null) throw new ArgumentException($"{nameof(type)} must not be null");
        if (type.IsAssignableTo(typeof(IConsoleMenu)) == false) throw new ArgumentException($"{type.Name} is not assignable to IConsoleTool");

        services.AddTransient(type);
        services.AddTransient(x => (IConsoleMenu)x.GetRequiredService(type));

        return services;
    }
}
