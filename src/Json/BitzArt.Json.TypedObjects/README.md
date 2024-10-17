[![NuGet version](https://img.shields.io/nuget/v/BitzArt.Json.TypedObjects.svg)](https://www.nuget.org/packages/BitzArt.Json.TypedObjects/)
[![NuGet downloads](https://img.shields.io/nuget/dt/BitzArt.Json.TypedObjects.svg)](https://www.nuget.org/packages/BitzArt.Json.TypedObjects/)

## Overview
`BitzArt.Json.TypedObjects` provides a way to retain actual type of values during serialization and deserialization.

## The problem
Serialization can encounter issues when dealing with interfaces, abstract and base classes, or `object`, etc. The resulting JSON will not include information about the specific types of values. This becomes a problem during deserialization, as the actual type will be lost, and the deserialized value will not match its original type.

### Example
Consider the following class hierarchy:

```csharp
public class Animal { }

public class Dog : Animal { } 

public class Cat : Animal { }
```

Serializing and deserializing instances of `Dog` and `Cat` as `Animal` will result in loss of their actual types:

```csharp
var animals = new List<Animal> { new Dog(), new Cat() };
var serialized = JsonSerializer.Serialize(animals);

// The deserialized list contains instances of Animal.
var deserialized = JsonSerializer.Deserialize<List<Animal>>(serialized);
```

## The solution
`TypedObjectJsonConverter` is a JSON converter designed to handle serialization and deserialization retaining original type of values:

 - __Serialization__: when a value is serialized, `TypedObjectJsonConverter` stores value's full type name along with the value itself in the resulting JSON.

- __Deserialization__: when reading from the JSON, `TypedObjectJsonConverter` uses full type name to resolve the original type of the value.

JSON of the list from previous example serialized with `TypedObjectJsonConverter` will have following structure:

```json
[
	{
		"type": "Namespace.Dog",
		"value": { }
	},
	{
		"type": "Namespace.Cat",
		"value": { }
	}
]
```
