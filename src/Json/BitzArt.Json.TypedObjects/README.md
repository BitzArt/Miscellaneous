[![NuGet version](https://img.shields.io/nuget/v/BitzArt.Json.TypedObjects.svg)](https://www.nuget.org/packages/BitzArt.Json.TypedObjects/)
[![NuGet downloads](https://img.shields.io/nuget/dt/BitzArt.Json.TypedObjects.svg)](https://www.nuget.org/packages/BitzArt.Json.TypedObjects/)

## Overview

**BitzArt.Json.TypedObjects** provides utilities for preserving the original type information during serialization and deserialization of objects and collections of objects.

## Installation

- Install the following package to your project:

```
dotnet add package BitzArt.Json.TypedObjects
```

## Usage

### TypedObjectJsonConverter

To serialize and deserialize an object property with an ability to preserve its original type information, 
apply the `JsonConverter` attribute with the `TypedObjectJsonConverter` to the property:

```csharp
public class YourClass
{
	[JsonConverter(typeof(TypedObjectJsonConverter<object>))]
	public object Value { get; set; }
}
```

During serialization and deserialization, the `Value` property preserves its original type information.

```csharp
var myClass = new YourClass { Value = 42 };
var serialized = JsonSerializer.Serialize(myClass);

var deserialized = JsonSerializer.Deserialize<YourClass>(serialized);
var type = deserialized.Value.GetType(); // System.Int32
```

### ItemConverter

To serialize and deserialize a collection of objects with an ability to preserve the original type information, apply the `JsonConverter` attribute with the `ItemConverter`, specifying `TypedObjectJsonConverter` as its generic type to the property:

```csharp
public class YourClass
{
	[JsonConverter(typeof(ItemConverter<TypedObjectJsonConverter<object>>))]
	public IEnumerable<object> Values { get; set; }
}
```

During serialization and deserialization, items of `Values` property preserve their original types information.

```csharp
var myClass = new YourClass { Values = [ 42, "Hello"] };
var serialized = JsonSerializer.Serialize(myClass);

var deserialized = JsonSerializer.Deserialize<YourClass>(serialized);
var types = deserialized.Values.Select(x => x.GetType()); // System.Int32, System.String
```

## License

[![License](https://img.shields.io/badge/mit-%230072C6?style=for-the-badge)](https://github.com/BitzArt/Miscellaneous/blob/main/LICENSE)
