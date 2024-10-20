[![NuGet version](https://img.shields.io/nuget/v/BitzArt.Json.TypedObjects.svg)](https://www.nuget.org/packages/BitzArt.Json.TypedObjects/)
[![NuGet downloads](https://img.shields.io/nuget/dt/BitzArt.Json.TypedObjects.svg)](https://www.nuget.org/packages/BitzArt.Json.TypedObjects/)

# Overview

`BitzArt.Json.TypedObjects` is a nuget package that allows retaining actual value types during JSON serialization and deserialization.

Since JSON does not preserve information about specific value types, serialization of such values may lead to this 
information being lost in cases where the actual type is not known in compile-time.

> ⚠️
> Currently, the library only supports [standard value types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/types).

## The Problem

Consider the following example:

```csharp
List<object> myObjects = [1, 1.234, "string", null, new Apple()];
```

What will happen if we serialize this list to JSON and then deserialize it back?

```csharp
var serialized = JsonSerializer.Serialize(myObjects);
var deserialized = JsonSerializer.Deserialize<List<object>>(serialized);
```

The resulting list will contain objects of type `JsonElement` instead of their original types, and the information about the objects' actual types will be lost - since the `JsonConverter` does not know the actual types of the objects in the list, and cannot deserialize them correctly.

### Polymorphic Types

Now, let's consider the following class hierarchy:

```csharp
public class Fruit { }

public class Apple : Fruit
{
    public string Variety { get; set; }
}
```

The following code will result in the loss of the actual type of the object when deserialized:

```csharp
var apple = new Apple
{
    Variety = "Granny Smith"
};

var serialized = JsonSerializer.Serialize(apple);

var deserialized = JsonSerializer.Deserialize<Fruit>(serialized);
```

Since the object was deserialized as `Fruit`, the resulting object will be of type `Fruit`, and all properties specific to the `Apple` class will be lost.

## The Solution

This library implements `TypedObjectJsonConverter` - a custom [JSON converter](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/converters-how-to) 
that handles serialization and deserialization of polymorphic types, retaining actual value types:

Now, let's see how the `TypedObjectJsonConverter` can be used to solve the problem of losing actual types during serialization and deserialization:

```csharp
var apple = new Apple
{
    Variety = "Granny Smith"
};

var fruit = (Fruit)apple; 

var jsonSerializerOptions = new JsonSerializerOptions
{
    Converters = { new TypedObjectJsonConverter<Fruit>() }
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