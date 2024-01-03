using BitzArt.Console;
using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.SpectreConsole;

internal class Program
{
    static void Main()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddConsoleMenusFromAssemblyContaining<Program>();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        serviceProvider.RunConsoleMenu<MainMenu>();
    }
}
