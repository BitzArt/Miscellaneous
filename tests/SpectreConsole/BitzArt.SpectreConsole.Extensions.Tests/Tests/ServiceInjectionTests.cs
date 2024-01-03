using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.ConsoleTools.Tests;

public class ServiceInjectionTests
{
    [Fact]
    public void AddConsoleTool_TestTool1_Adds()
    {
        var services = new ServiceCollection();

        services.AddConsoleMenu<TestTool1>();

        var provider = services.BuildServiceProvider();

        var tools = provider.GetServices<IConsoleMenu>();
        Assert.Single(tools);

        var toolByInterface = provider.GetService<IConsoleMenu>();
        Assert.NotNull(toolByInterface);
        Assert.True(toolByInterface is TestTool1);

        var toolByType = provider.GetService<TestTool1>();
        Assert.NotNull(toolByType);
        Assert.True(toolByType is IConsoleMenu);
    }

    [Fact]
    public void AddConsoleTool_TwoGenericCalls_AddsBoth()
    {
        var services = new ServiceCollection();

        services.AddConsoleMenu<TestTool1>();
        services.AddConsoleMenu<TestTool2>();

        var provider = services.BuildServiceProvider();

        var tools = provider.GetServices<IConsoleMenu>();

        Assert.Equal(2, tools.Count());
        Assert.Contains(tools, x => x.GetType() == typeof(TestTool1));
        Assert.Contains(tools, x => x.GetType() == typeof(TestTool2));
    }

    [Fact]
    public void AddConsoleTool_TwoReflectionCalls_AddsBoth()
    {
        var services = new ServiceCollection();

        services.AddConsoleMenu(typeof(TestTool1));
        services.AddConsoleMenu(typeof(TestTool2));

        var provider = services.BuildServiceProvider();

        var tools = provider.GetServices<IConsoleMenu>();

        Assert.Equal(2, tools.Count());
        Assert.Contains(tools, x => x.GetType() == typeof(TestTool1));
        Assert.Contains(tools, x => x.GetType() == typeof(TestTool2));
    }

    [Fact]
    public void AddConsoleToolsFromAssembly_ThisAssembly_AddsBoth()
    {
        var services = new ServiceCollection();

        services.AddConsoleMenusFromAssembly(typeof(ServiceInjectionTests).Assembly);

        var provider = services.BuildServiceProvider();

        var tools = provider.GetServices<IConsoleMenu>();

        Assert.Equal(2, tools.Count());
        Assert.Contains(tools, x => x.GetType() == typeof(TestTool1));
        Assert.Contains(tools, x => x.GetType() == typeof(TestTool2));
    }

    [Fact]
    public void AddConsoleToolsFromAssemblyContaining_Typeof_AddsBoth()
    {
        var services = new ServiceCollection();

        services.AddConsoleMenusFromAssemblyContaining(typeof(ServiceInjectionTests));

        var provider = services.BuildServiceProvider();

        var tools = provider.GetServices<IConsoleMenu>();

        Assert.Equal(2, tools.Count());
        Assert.Contains(tools, x => x.GetType() == typeof(TestTool1));
        Assert.Contains(tools, x => x.GetType() == typeof(TestTool2));
    }

    [Fact]
    public void AddConsoleToolsFromAssemblyContaining_Generic_AddsBoth()
    {
        var services = new ServiceCollection();

        services.AddConsoleMenusFromAssemblyContaining<ServiceInjectionTests>();

        var provider = services.BuildServiceProvider();

        var tools = provider.GetServices<IConsoleMenu>();

        Assert.Equal(2, tools.Count());
        Assert.Contains(tools, x => x.GetType() == typeof(TestTool1));
        Assert.Contains(tools, x => x.GetType() == typeof(TestTool2));
    }
}
