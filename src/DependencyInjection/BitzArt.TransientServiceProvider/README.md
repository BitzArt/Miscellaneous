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
You need to pass `Action<TransientServiceProviderOptions>` to the `AddTransientServiceProvider` extension method and use `TransientServiceScopeServiceProviderFactory` for `IHostBuilder`.

You can find a configuration example in the [Startup](../../../tests/DependencyInjection/BitzArt.TransientServiceProvider.XUnitDependencyInjectionTests/Startup.cs) file of the BitzArt.TransientServiceProvider.XUnitDependencyInjectionTests project.

The algorithm used to resolve dependencies.