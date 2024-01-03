using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.Console;

public static class RunConsoleMenuExtension
{
    public static void RunConsoleMenu<TConsoleTool>(this IServiceProvider serviceProvider)
        where TConsoleTool : IConsoleMenu
    {
        var tool = serviceProvider.GetRequiredService<TConsoleTool>();

        tool.Run();
    }

    public static void RunConsoleMenu(this IServiceProvider serviceProvider, Type type)
    {
        if (!type.IsAssignableTo(typeof(IConsoleMenu))) throw new ArgumentException($"Type must implement {nameof(IConsoleMenu)} interface", nameof(type));
        
        var tool = (IConsoleMenu)serviceProvider.GetRequiredService(type);

        tool.Run();
    }
}
