using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.ConsoleTools.Tests;

public class ServiceInjectionTests
{
    [Fact]
    public void AddConsoleTool_TestTool1_Adds()
    {
        var services = new ServiceCollection();

        services.AddConsoleTool<TestTool1>();

        var provider = services.BuildServiceProvider();

        var tools = provider.GetServices<IConsoleTool>();
        Assert.Single(tools);

        var toolByInterface = provider.GetService<IConsoleTool>();
        Assert.NotNull(toolByInterface);
        Assert.True(toolByInterface is TestTool1);

        var toolByType = provider.GetService<TestTool1>();
        Assert.NotNull(toolByType);
        Assert.True(toolByType is IConsoleTool);
    }

    [Fact]
    public void AddConsoleTool_TwoGenericCalls_AddsBoth()
    {
        var services = new ServiceCollection();

        services.AddConsoleTool<TestTool1>();
        services.AddConsoleTool<TestTool2>();

        var provider = services.BuildServiceProvider();

        var tools = provider.GetServices<IConsoleTool>();

        Assert.Equal(2, tools.Count());
        Assert.Contains(tools, x => x.GetType() == typeof(TestTool1));
        Assert.Contains(tools, x => x.GetType() == typeof(TestTool2));
    }

    [Fact]
    public void AddConsoleTool_TwoReflectionCalls_AddsBoth()
    {
        var services = new ServiceCollection();

        services.AddConsoleTool(typeof(TestTool1));
        services.AddConsoleTool(typeof(TestTool2));

        var provider = services.BuildServiceProvider();

        var tools = provider.GetServices<IConsoleTool>();

        Assert.Equal(2, tools.Count());
        Assert.Contains(tools, x => x.GetType() == typeof(TestTool1));
        Assert.Contains(tools, x => x.GetType() == typeof(TestTool2));
    }

    [Fact]
    public void AddConsoleToolsFromAssembly_ThisAssembly_AddsBoth()
    {
        var services = new ServiceCollection();

        services.AddConsoleToolsFromAssembly(typeof(ServiceInjectionTests).Assembly);

        var provider = services.BuildServiceProvider();

        var tools = provider.GetServices<IConsoleTool>();

        Assert.Equal(2, tools.Count());
        Assert.Contains(tools, x => x.GetType() == typeof(TestTool1));
        Assert.Contains(tools, x => x.GetType() == typeof(TestTool2));
    }

    [Fact]
    public void AddConsoleToolsFromAssemblyContaining_Typeof_AddsBoth()
    {
        var services = new ServiceCollection();

        services.AddConsoleToolsFromAssemblyContaining(typeof(ServiceInjectionTests));

        var provider = services.BuildServiceProvider();

        var tools = provider.GetServices<IConsoleTool>();

        Assert.Equal(2, tools.Count());
        Assert.Contains(tools, x => x.GetType() == typeof(TestTool1));
        Assert.Contains(tools, x => x.GetType() == typeof(TestTool2));
    }

    [Fact]
    public void AddConsoleToolsFromAssemblyContaining_Generic_AddsBoth()
    {
        var services = new ServiceCollection();

        services.AddConsoleToolsFromAssemblyContaining<ServiceInjectionTests>();

        var provider = services.BuildServiceProvider();

        var tools = provider.GetServices<IConsoleTool>();

        Assert.Equal(2, tools.Count());
        Assert.Contains(tools, x => x.GetType() == typeof(TestTool1));
        Assert.Contains(tools, x => x.GetType() == typeof(TestTool2));
    }
}
