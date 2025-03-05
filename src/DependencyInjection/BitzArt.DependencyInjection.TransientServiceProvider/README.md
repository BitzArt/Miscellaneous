[![NuGet version](https://img.shields.io/nuget/v/BitzArt.DependencyInjection.TransientServiceProvider.svg)](https://www.nuget.org/packages/BitzArt.DependencyInjection.TransientServiceProvider/)
[![NuGet downloads](https://img.shields.io/nuget/dt/BitzArt.DependencyInjection.TransientServiceProvider.svg)](https://www.nuget.org/packages/BitzArt.DependencyInjection.TransientServiceProvider/)

# Overview

`BitzArt.DependencyInjection.TransientServiceProvider` is a NuGet package that enables the creation and management of transient service providers within a dependency injection container.

It allows resolving services within isolated transient scopes while supporting both named and unnamed providers. This ensures proper service lifetime management without polluting the root provider.

> ⚠️
> Currently, the library requires .NET 9 and is not compatible with earlier versions. Ensure your project targets .NET 9 or later to use this package.


## The Problem

When tests share the same AppDbContext and UserService instances, data modifications in one test can interfere with another, causing unexpected failures.

```csharp
public class SomeTests(AppDbContext context, UserService service)
{
    [Fact]
    public async Task ActivateUserAsync_UserExists_ShouldSetActive()
    {
        // Arrange: Seed an inactive user
        context.Users.Add(new User { Name = "Alice", IsActive = false });
        await context.SaveChangesAsync();

        // Act: Activate the user
        await service.ActivateUserAsync("Alice");

        // Assert: Check the user is active
        var alice = await context.Users.SingleAsync(u => u.Name == "Alice");
        Assert.True(alice.IsActive);
    }

    [Fact]
    public async Task DeleteInactiveUsersAsync_InactiveUserExists_ShouldRemainOnlyActive()
    {
        // Arrange: Seed some users
        context.Users.AddRange(
            new User { Name = "Bob", IsActive = true },
            new User { Name = "Charlie", IsActive = false }
        );
        await context.SaveChangesAsync();

        // Act: Delete inactive users
        await service.DeleteInactiveUsersAsync();

        // Assert: Only active users remain
        var remainingCount = await context.Users.Where(x => x.IsActive).CountAsync();
        Assert.Equal(1, remainingCount);
    }
}
```
In this case, first test seeds an inactive Alice and tests ActivateUserAsync method, expecting her to become active. Second test seeds Bob (active) and Charlie (inactive) and tests DeleteInactiveUsersAsync, expecting only Bob to remain.
With a shared AppDbContext, Charlie from second test might get activated by first test, or Alice from first test might get deleted by test 2, leading to unpredictable failures.


## The Solution

By using TransientServiceProviderFactory, each test obtains its own isolated AppDbContext and UserService instances. This prevents one test's changes from affecting another.

```csharp
public class SomeTests(TransientServiceProviderFactory factory)
{
    [Fact]
    public async Task ActivateUserAsync_UserExists_ShouldSetActive()
    {
        var provider = factory.GetProvider(nameof(ActivateUserAsync_UserExists_ShouldSetActive));
        var context = provider.GetRequiredService<AppDbContext>();
        var service = provider.GetRequiredService<UserService>();

        // Arrange: Seed an inactive user
        context.Users.Add(new User { Name = "Alice", IsActive = false });
        await context.SaveChangesAsync();

        // Act: Activate the user
        await service.ActivateUserAsync("Alice");

        // Assert: Check the user is active
        var alice = await context.Users.SingleAsync(u => u.Name == "Alice");
        Assert.True(alice.IsActive);
    }

    [Fact]
    public async Task DeleteInactiveUsersAsync_InactiveUserExists_ShouldRemainOnlyActive()
    {
        var provider = factory.GetProvider(nameof(DeleteInactiveUsersAsync_InactiveUserExists_ShouldRemainOnlyActive));
        var context = provider.GetRequiredService<AppDbContext>();
        var service = provider.GetRequiredService<UserService>();

        // Arrange: Seed some users
        context.Users.AddRange(
            new User { Name = "Bob", IsActive = true },
            new User { Name = "Charlie", IsActive = false }
        );
        await context.SaveChangesAsync();

        // Act: Delete inactive users
        await service.DeleteInactiveUsersAsync();

        // Assert: Only active users remain
        var remainingCount = await context.Users.Where(x => x.IsActive).CountAsync();
        Assert.Equal(1, remainingCount);
    }
}
```


## How it Works

### Centralized Provider Factory:

The package introduces a factory that holds a reference to the root service provider and a delegate function to create transient providers.

### Named and Unnamed Providers:

- Named Providers: Uses a thread-safe cache (a concurrent dictionary) to store and retrieve providers by name. If a provider for a given name doesn’t exist, it is created, cached, and then returned.
- Unnamed Providers: Always creates a new transient provider instance using the delegate function.

### Thread Safety:

The factory employs locking mechanisms to ensure that the creation and caching of named providers occur safely when accessed concurrently.

### Isolation and Flexibility:

This approach allows different parts of an application to obtain isolated service provider instances, ensuring that transient services do not interfere with one another.


## Configure

The following demonstrates how to configure and use the `TransientServiceProvider`.

### Here's How to Configure It

Below is an example of how to set up the `TransientServiceProvider` within your application's service registration:

```csharp
services.AddTransientServiceProvider(sp =>
    {
        // Set up a fresh service collection for registration.
        var services = new ServiceCollection();

        // Add services, database contexts, or other dependencies here.

        // Construct a service provider from the configured collection.
        var innerServiceProvider = services.BuildServiceProvider();
        
        // Optionally, utilize the newly created provider if needed.

        // Provide a new TransientServiceProvider instance with the inner provider.
        return new TransientServiceProvider(innerServiceProvider);
    });
```

### Here's How to Use It

Once the `TransientServiceProviderFactory` is configured, you can inject it into your classes and call `GetProvider()` to create isolated service providers as needed. Here’s an example:

```csharp
public class SomeClass(TransientServiceProviderFactory factory)
{
    public async Task SomeMethod()
    {
        // Get an isolated service provider
        using var serviceProvider = factory.GetProvider();
        
        // Resolve the service
        var service = serviceProvider.GetRequiredService<SomeService>();

        // Call the service method
        await service.SomeServiceMethod();
    }
}
```

You can also create or retrieve named isolated service providers by calling GetProvider("some-name"). If a provider with that name already exists, it will return the existing instance rather than creating a new one. Here’s an example:

```csharp
public class AnotherClass(TransientServiceProviderFactory factory)
{
    public async Task AnotherMethod()
    {
        // Get or reuse a named isolated service provider
        using var namedProvider = factory.GetProvider("MyNamedProvider");
        
        // Resolve the service from the named provider
        var service = namedProvider.GetRequiredService<SomeService>();

        // Call the service method
        await service.SomeServiceMethod();
    }
}
```