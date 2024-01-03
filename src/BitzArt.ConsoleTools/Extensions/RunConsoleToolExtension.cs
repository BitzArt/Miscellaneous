using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.ConsoleTools;

public static class RunConsoleToolExtension
{
    public static void RunConsoleTool<TConsoleTool>(this IServiceProvider serviceProvider)
        where TConsoleTool : IConsoleTool
    {
        var tool = serviceProvider.GetRequiredService<TConsoleTool>();

        tool.Run();
    }

    public static void RunConsoleTool(this IServiceProvider serviceProvider, Type type)
    {
        if (!type.IsAssignableTo(typeof(IConsoleTool))) throw new ArgumentException($"Type must implement {nameof(IConsoleTool)} interface", nameof(type));
        
        var tool = (IConsoleTool)serviceProvider.GetRequiredService(type);

        tool.Run();
    }
}
