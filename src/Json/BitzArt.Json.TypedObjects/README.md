[![NuGet version](https://img.shields.io/nuget/v/BitzArt.Json.TypedObjects.svg)](https://www.nuget.org/packages/BitzArt.Json.TypedObjects/)
[![NuGet downloads](https://img.shields.io/nuget/dt/BitzArt.Json.TypedObjects.svg)](https://www.nuget.org/packages/BitzArt.Json.TypedObjects/)

# Overview

`BitzArt.Json.TypedValues` is a nuget package that allows retaining type information when serializing and deserializing values to and from JSON using `System.Text.Json`.

> ⚠️
> Currently, the library only supports persisting type information for [standard value types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/types).

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
var apple = new Apple
{
    AppleVariety = "Granny Smith"
};

var serialized = JsonSerializer.Serialize(apple);

var deserialized = JsonSerializer.Deserialize<Fruit>(serialized);
```

Since the object was deserialized as `Fruit`, the resulting object will be of type `Fruit`, and all properties specific to the `Apple` class will be lost.

## The Solution

This library implements `TypedValueJsonConverter` - a custom [JSON converter](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/converters-how-to) 
that handles serialization and deserialization of polymorphic types, retaining their type information.

Now, let's see how the `TypedValueJsonConverter` can be used to solve the problem of persisting type information during serialization and deserialization:

```csharp
var fruits = new List<Fruit>
{
    new Apple { AppleVariety = "Granny Smith" },
    new Banana { BananaVariety = "Cavendish" }
};

var jsonSerializerOptions = new JsonSerializerOptions
{
    Converters = { new TypedValueJsonConverter<Fruit>() }
};

var serialized = JsonSerializer.Serialize(fruit, jsonSerializerOptions);

var deserialized = JsonSerializer.Deserialize<Fruit>(serialized, jsonSerializerOptions);
```

## How it Works

When a value is serialized, `TypedObjectJsonConverter` stores the value's type name along with the value itself in the resulting JSON:

```json
{
    "type": "MyNamespace.MyClass",
    "value": {"The actual serialized value of the object goes here"}
}
```

When deserializing, the `TypedObjectJsonConverter` uses the stored type name to find the actual type of the object via reflection, and then uses it to deserialize the value, thus retaining the actual type of the object.