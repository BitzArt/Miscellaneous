[![NuGet version](https://img.shields.io/nuget/v/BitzArt.Json.TypedObjects.svg)](https://www.nuget.org/packages/BitzArt.Json.TypedObjects/)
[![NuGet downloads](https://img.shields.io/nuget/dt/BitzArt.Json.TypedObjects.svg)](https://www.nuget.org/packages/BitzArt.Json.TypedObjects/)

## Overview

`BitzArt.Json.TypedObjects` provides solution to retain actual types of values during JSON serialization and deserialization.

> ⚠️ The current version of the library only supports polymorphic serialization and deserialization of 
> [C# standard value types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/types).

## The problem 

Since JSON does not preserve information about specific value types, serialization of such values may lead to this 
information being lost in cases where the actual type is not known in compile-time, e.g. when operating with a value 
of a specific type as it's base type, like `var fruit = (new Apple() as Fruit);`

### Example

```csharp
public class Fruit { }

public class Apple : Fruit
{
	public string? Variety { get; set; }
}
```

```csharp
var apple = new Apple
{
	Variety = "Granny Smith"
};

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
 
For instance, JSON output for the object in the previous [example](#example) when serialized with `TypedObjectJsonConverter` 
would have the following structure:

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
var apple = new Apple
{
	Variety = "Granny Smith"
};

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