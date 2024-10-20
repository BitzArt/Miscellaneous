[![NuGet version](https://img.shields.io/nuget/v/BitzArt.Json.TypedObjects.svg)](https://www.nuget.org/packages/BitzArt.Json.TypedObjects/)
[![NuGet downloads](https://img.shields.io/nuget/dt/BitzArt.Json.TypedObjects.svg)](https://www.nuget.org/packages/BitzArt.Json.TypedObjects/)

## Overview

`BitzArt.Json.TypedObjects` provides solution to retain actual types of values during JSON serialization and deserialization.

> ⚠️ The current version of the libraty only supports polymorphic serialization and deserialization of 
> [C# standard value types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/types).

## The problem 

Since JSON does not preserve information about specific value types, serialization and deserialization of polymorphic types cause values to lose their actual types.

### Example

Consider the following classes:

```csharp
public class Fruit { }

public class Apple(string variety) : Fruit
{
	public string? Variety { get; set; } = variety;
}
```

If for some reason the actual type of an object not known at compile time, 
it can be deserialized using it's base type, but all properties specific to the actual type will be lost:

```csharp
var apple = new Apple("Granny Smith");

// Serialized as Apple
var serialized = JsonSerializer.Serialize(apple);

// This will result in a Fruit object.
// All properties specific to the Apple class will be lost
var deserialized = JsonSerializer.Deserialize<Fruit>(serialized);
```

## The solution

`TypedObjectJsonConverter` is a [custom JSON converter](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/converters-how-to) 
designed to handle serialization and deserialization of polymorphic types retaining actual types of values:

 - __Serialization__: when a value is serialized, `TypedObjectJsonConverter` stores value's full type name along with the value itself in the resulting JSON. 
 
 The JSON of a value serialized with `TypedObjectJsonConverter` has the following structure:

```json
{
	"type": "Namespace.Apple",
	"value": {
		"variety": "Granny Smith"
	}
},
```

- __Deserialization__: when reading from JSON, `TypedObjectJsonConverter` uses `type` and deserializes `value` polymorphically to the actual type.

### Example

```csharp
var apple = new Apple("Granny Smith");
var fruit = (Fruit)apple; 

// TypedObjectJsonConverter can be added to the JsonSerializerOptions
// to solve the issue of losing actual types during serialization and deserialization.
var jsonSerializerOptions = new JsonSerializerOptions
{
	Converters = { new TypedObjectJsonConverter<Fruit>() }
};

// The converter will retain the actual type of the object when serializing
var serializedWithConverter = JsonSerializer.Serialize(fruit, jsonSerializerOptions);

// Now, when deserializing, the actual type of the object also will be retained,
// even if the type is not known at compile time.
var deserializedWithConverter = JsonSerializer.Deserialize<Fruit>(serializedWithConverter, jsonSerializerOptions);

// All properties specific to the Apple class will be restored from the JSON
Apple deserializedAppleWithConverter = (Apple)deserializedWithConverter!;
var variety = deserializedAppleWithConverter.Variety; // "Granny Smith"
```