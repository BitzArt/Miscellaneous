[![NuGet version](https://img.shields.io/nuget/v/BitzArt.Json.TypedObjects.svg)](https://www.nuget.org/packages/BitzArt.Json.TypedObjects/)
[![NuGet downloads](https://img.shields.io/nuget/dt/BitzArt.Json.TypedObjects.svg)](https://www.nuget.org/packages/BitzArt.Json.TypedObjects/)

## Overview
`BitzArt.Json.TypedObjects` provides a way to retain actual types of values during serialization and deserialization.

## The problem
Serialization can encounter issues when dealing with interfaces, abstract and base classes, or `object`, etc. The resulting JSON will not include information about the specific types of values. This becomes a problem during deserialization, as the actual type will be lost, and the deserialized value will not match its original type.

### Example
Consider the following class hierarchy:

```csharp
public interface IShape { }

public class Rectangle(int width, int height) : IShape
{
    public int Width { get; set; } = width;
    public int Height { get; set; } = height;
}

public class Circle(int radius) : IShape
{
    public int Radius { get; set; } = radius;
}
```

Serializing and deserializing instances of `Rectangle` and `Circle` as `IShape` will result in loss of their actual types:

```csharp
var shapes = new List<IShape> { new Rectangle(16, 9), new Circle(5) };
var serialized = JsonSerializer.Serialize(shapes);

// The deserialized list contains IShape
var deserialized = JsonSerializer.Deserialize<List<IShape>>(serialized);
```

## The solution
`TypedObjectJsonConverter` is a JSON converter designed to handle serialization and deserialization retaining actual types of values:

 - __Serialization__: when a value is serialized, `TypedObjectJsonConverter` stores value's full type name along with the value itself in the resulting JSON.

- __Deserialization__: when reading from the JSON, `TypedObjectJsonConverter` uses full type name to resolve the actual type of the value.

JSON of the list from previous [example](#example) serialized with `TypedObjectJsonConverter` will have following structure:

```json
[
	{
		"type": "Namespace.Rectangle",
		"value": {
			"width": 16,
			"height": 9
		}
	},
	{
		"type": "Namespace.Circle",
		"value": {
			"radius": 5
		}
	}
]
```
