[![NuGet version](https://img.shields.io/nuget/v/BitzArt.Json.TypedValues.svg)](https://www.nuget.org/packages/BitzArt.Json.TypedValues/)
[![NuGet downloads](https://img.shields.io/nuget/dt/BitzArt.Json.TypedValues.svg)](https://www.nuget.org/packages/BitzArt.Json.TypedValues/)

# Overview

`BitzArt.Json.TypedValues` is a nuget package that allows retaining actual type information when serializing and deserializing values to and from JSON using `System.Text.Json`.

## The Problem

Let's consider the following class hierarchy:

```csharp
public class Fruit { }

public class Apple : Fruit
{
    public string AppleVariety { get; set; }
}

public class Banana : Fruit
{
    public string BananaVariety { get; set; }
}
```

The following code will result in the loss of the actual type of the object when deserialized:

```csharp
var fruits = new List<Fruit>
{
    new Apple { AppleVariety = "Granny Smith" },
    new Banana { BananaVariety = "Cavendish" }
};

var serialized = JsonSerializer.Serialize(fruits);

// Resulting deserialized objects are losing their
// actual types since the type information is not retained in JSON.
var deserialized = JsonSerializer.Deserialize<IEnumerable<Fruit>>(serialized);
```

## The Solution

`BitzArt.Json.TypedValues` provides a way to retain type information during serialization and deserialization. By using the `TypedValue<T>` class, you can ensure that the type information is preserved.

```csharp
var fruits = new List<TypedValue<Fruit>>
{
    new Apple { AppleVariety = "Granny Smith" },
    new Banana { BananaVariety = "Cavendish" }
};

var serialized = JsonSerializer.Serialize(fruits);

// Resulting deserialized objects will be deserialized back to their original types.
var deserialized = JsonSerializer.Deserialize<IEnumerable<TypedValue<Fruit>>>(serialized);
// or
var deserialized = JsonSerializer.Deserialize<IEnumerable<TypedValue>>(serialized).Select(x => (Fruit)x.Value);
```