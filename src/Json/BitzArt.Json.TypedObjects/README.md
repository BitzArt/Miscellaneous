[![NuGet version](https://img.shields.io/nuget/v/BitzArt.Json.TypedObjects.svg)](https://www.nuget.org/packages/BitzArt.Json.TypedObjects/)
[![NuGet downloads](https://img.shields.io/nuget/dt/BitzArt.Json.TypedObjects.svg)](https://www.nuget.org/packages/BitzArt.Json.TypedObjects/)

## Overview
`BitzArt.Json.TypedObjects` provides solution to retain actual types of values during JSON serialization and deserialization.

## The problem 
Since JSON does not preserve information about specific value types, serialization and deserialization of polymorphic types cause values to lose their actual types.

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

If you serialize and deserialize a list of `Rectangle` and `Circle` as a list of `IShape`, actual types of items will be lost:

```csharp
var shapes = new List<IShape> { new Rectangle(16, 9), new Circle(5) };
var serialized = JsonSerializer.Serialize(shapes);

// The deserialized list contains IShape instances
var deserialized = JsonSerializer.Deserialize<List<IShape>>(serialized);
```

## The solution
`TypedObjectJsonConverter` is a [custom JSON converter](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/converters-how-to) designed to handle serialization and deserialization of polymorphic types retaining actual types of values:

 - __Serialization__: when a value is serialized, `TypedObjectJsonConverter` stores value's full type name along with the value itself in the resulting JSON. 
 
 The JSON of a value serialized with `TypedObjectJsonConverter` has the following structure:

```json
{
	"type": "Namespace.Rectangle",
	"value": {
		"width": 16,
		"height": 9
	}
},
```

- __Deserialization__: when reading from JSON, `TypedObjectJsonConverter` uses `type` and deserializes `value` polymorphically to the actual type.


