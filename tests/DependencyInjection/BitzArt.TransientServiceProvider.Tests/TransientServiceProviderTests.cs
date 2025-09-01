using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.TransientServiceProvider.Tests;

public class TransientServiceProviderTests
{
    private class MyDependency(int value)
    {
        public int Value { get; set; } = value;
    }

    [Fact]
    public void GetService_ViaFallback_ShouldReturnServiceFromGlobal()
    {
        // Arrange
        var globalServiceCollection = new ServiceCollection();
        const int value = 15;

        globalServiceCollection.AddSingleton(new MyDependency(value));

        // Act
        globalServiceCollection.AddTransientServiceProvider(services =>
        {

        },
        configureOptions: options => {
            options.FallbackToGlobal = true;
        });

        // Assert
        using var globalServiceProvider = globalServiceCollection.BuildServiceProvider();
        var transientServiceProvider = globalServiceProvider.GetRequiredService<ITransientServiceProvider>();

        var myDependency = transientServiceProvider.GetRequiredService<MyDependency>();

        Assert.NotNull(myDependency);
        Assert.Equal(value, myDependency.Value);

    }

    [Fact]
    public void GetEnumerableService_ViaFallback_ShouldReturnEnumerableServiceFromGlobal()
    {
        // Arrange
        var globalServiceCollection = new ServiceCollection();
        const int value = 15;

        globalServiceCollection.AddSingleton(new MyDependency(value));

        // Act
        globalServiceCollection.AddTransientServiceProvider(services =>
        {

        },
        configureOptions: options => {
            options.FallbackToGlobal = true;
        });

        // Assert
        using var globalServiceProvider = globalServiceCollection.BuildServiceProvider();
        var transientServiceProvider = globalServiceProvider.GetRequiredService<ITransientServiceProvider>();

        var myDependency = transientServiceProvider.GetRequiredService<IEnumerable<MyDependency>>();

        Assert.NotNull(myDependency);
        Assert.Single(myDependency);
        Assert.Equal(value, myDependency.Single().Value);

    }

    [Fact]
    public void GetService_WithFallbackFalseAndTransientMissingService_ShouldReturnNull()
    {
        // Arrange
        var globalServiceCollection = new ServiceCollection();
        const int value = 15;

        globalServiceCollection.AddSingleton(new MyDependency(value));

        // Act
        globalServiceCollection.AddTransientServiceProvider(services =>
        {

        },
        configureOptions: options => {
            options.FallbackToGlobal = false;
        });

        // Assert
        using var globalServiceProvider = globalServiceCollection.BuildServiceProvider();
        var transientServiceProvider = globalServiceProvider.GetRequiredService<ITransientServiceProvider>();

        var myDependency = transientServiceProvider.GetService<MyDependency>();

        Assert.Null(myDependency);
    }

    [Fact]
    public void GetEnumerableService_WithFallbackFalseAndTransientMissingService_ShouldReturnEmptyEnumerable()
    {
        // Arrange
        var globalServiceCollection = new ServiceCollection();
        const int value = 15;

        globalServiceCollection.AddSingleton(new MyDependency(value));

        // Act
        globalServiceCollection.AddTransientServiceProvider(services =>
        {

        },
        configureOptions: options => {
            options.FallbackToGlobal = false;
        });

        // Assert
        using var globalServiceProvider = globalServiceCollection.BuildServiceProvider();
        var transientServiceProvider = globalServiceProvider.GetRequiredService<ITransientServiceProvider>();

        var myDependency = transientServiceProvider.GetRequiredService<IEnumerable<MyDependency>>();

        Assert.NotNull(myDependency);
        Assert.Empty(myDependency);

    }
}
