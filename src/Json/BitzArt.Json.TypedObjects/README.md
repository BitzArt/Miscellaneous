[![NuGet version](https://img.shields.io/nuget/v/BitzArt.Json.TypedObjects.svg)](https://www.nuget.org/packages/BitzArt.Json.TypedObjects/)
[![NuGet downloads](https://img.shields.io/nuget/dt/BitzArt.Json.TypedObjects.svg)](https://www.nuget.org/packages/BitzArt.Json.TypedObjects/)

## Overview
`BitzArt.Json.TypedObjects` provides solution to retain actual types of values during JSON serialization and deserialization.

## The problem
Serializing using interfaces, abstract and base classes, or `object`, etc. results in JSON does not include information about the specific types of values, which causes issues during deserialization, as the actual types of the values are lost.

### Example
Consider the following classes implementing `IShape` interface:

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

If you serialize and deserialize a list of `Rectangle` and `Circle` objects as `IShape`, their actual types will be lost:

```csharp
var shapes = new List<IShape> { new Rectangle(16, 9), new Circle(5) };
var serialized = JsonSerializer.Serialize(shapes);

// The deserialized list contains IShape instances
var deserialized = JsonSerializer.Deserialize<List<IShape>>(serialized);
```

## The solution
`TypedObjectJsonConverter` is a custom JSON converter designed to handle serialization and deserialization retaining actual types of values:

 - __Serialization__: when a value is serialized, `TypedObjectJsonConverter` stores value's full type name along with the value itself in the resulting JSON.

- __Deserialization__: when reading from the JSON, `TypedObjectJsonConverter` uses full type name to resolve the actual type of the value.

For instance, JSON output for the list in the previous [example](#example) when serialized with `TypedObjectJsonConverter` would have the following structure:

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
