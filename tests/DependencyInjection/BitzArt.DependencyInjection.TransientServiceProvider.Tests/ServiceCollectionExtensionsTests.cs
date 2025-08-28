using Microsoft.Extensions.DependencyInjection;

namespace BitzArt.DependencyInjection.TransientServiceProvider.Tests;

public class ServiceCollectionExtensionsTests
{
    private class MyDependency(int value)
    {
        public int Value { get; set; } = value;
    }

    [Fact]
    public void AddTransientServiceProvider_WithServices_ShouldConfigureServiceProvider()
    {
        // Arrange
        var globalServiceCollection = new ServiceCollection();
        const int value = 15;

        // Act
        globalServiceCollection.AddTransientServiceProvider(services =>
        {
            services.AddSingleton(new MyDependency(value));
        });

        // Assert
        using var globalServiceProvider = globalServiceCollection.BuildServiceProvider();
        var transientServiceProvider = globalServiceProvider.GetRequiredService<ITransientServiceProvider>();

        var myDependency = transientServiceProvider.GetRequiredService<MyDependency>();
        Assert.NotNull(myDependency);

        Assert.Equal(value, myDependency.Value);
    }

    [Fact]
    public void AddTransientServiceProvider_UsingGlobalServiceProvider_ShouldConfigureServiceProvider()
    {
        // Arrange
        var globalServiceCollection = new ServiceCollection();
        const int value = 15;

        globalServiceCollection.AddSingleton(new MyDependency(value));

        // Act
        globalServiceCollection.AddTransientServiceProvider((services, global) =>
        {
            var myDependency = global.GetRequiredService<MyDependency>();
            services.AddSingleton(myDependency);
        });

        // Assert
        using var globalServiceProvider = globalServiceCollection.BuildServiceProvider();
        var transientServiceProvider = globalServiceProvider.GetRequiredService<ITransientServiceProvider>();
        var myDependency = transientServiceProvider.GetRequiredService<MyDependency>();
        Assert.NotNull(myDependency);

        Assert.Equal(value, myDependency.Value);
    }

    [Fact]
    public void AddTransientServiceProvider_Twice_ShouldThrow()
    {
        // Arrange
        var globalServiceCollection = new ServiceCollection();

        // Act + Assert
        globalServiceCollection.AddTransientServiceProvider(services =>
        {
            services.AddSingleton(new MyDependency(1));
        });

        Assert.ThrowsAny<Exception>(() =>
        {
            globalServiceCollection.AddTransientServiceProvider(services =>
            {
                services.AddSingleton(new MyDependency(2));
            });
        });
    }
}
