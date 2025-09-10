[![NuGet version](https://img.shields.io/nuget/v/BitzArt.DependencyInjection.TransientServiceProvider.svg)](https://www.nuget.org/packages/BitzArt.DependencyInjection.TransientServiceProvider/)
[![NuGet downloads](https://img.shields.io/nuget/dt/BitzArt.DependencyInjection.TransientServiceProvider.svg)](https://www.nuget.org/packages/BitzArt.DependencyInjection.TransientServiceProvider/)

# Overview

`BitzArt.DependencyInjection.TransientServiceProvider` is a NuGet package that allows the creation of transient service providers - each with its own set of services. Transient service providers are isolated from each other and from the default service provider.

## How it Works

### 1. Install

You can install the package via NuGet using the following command:

```bash
dotnet add package BitzArt.DependencyInjection.TransientServiceProvider
```

### 2. Configure

Add transient service provider factory to your service collection:

```csharp
services.AddTransientServiceProvider(serviceCollection =>
    {
        // Add services to the serviceCollection here.
    });
```

### 3. Resolve

Once the configuration is complete, you can resolve `ITransientServiceProvider`

- either directly from the root service provider

```csharp
var transient = serviceProvider.GetRequiredService<ITransientServiceProvider>();
```

- or by injecting it into one of your classes

```csharp
public class MyService(ITransientServiceProvider transientServiceProvider)
{
    // ...
}
```

### 4. Use

Use the resolved `ITransientServiceProvider` similarly to how you would use a normal `IServiceProvider`

```csharp
var myTransientService = transientServiceProvider.GetService<MyService>();
```

#### 4.1. Use `FallbackToGlobal` flag

You can use the `FallbackToGlobal` flag in the options configuration and inject dependencies directly.
If this `FallbackToGlobal` flag is enabled in the configuration, the system will first attempt to obtain the dependency from the `ITransientServiceProvider` container. This is useful when you need to override or isolate certain services in a specific context, for example, for testing or performing specific tasks.
If the required dependency is not found in `ITransientServiceProvider` and the `FallbackToGlobal` flag is enabled, the search will continue in the global default container.

You can find a configuration example in the [Startup](../../../tests/DependencyInjection/BitzArt.TransientServiceProvider.XUnitDependencyInjectionTests/Startup.cs) file of the BitzArt.TransientServiceProvider.XUnitDependencyInjectionTests project.