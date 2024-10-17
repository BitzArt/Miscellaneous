[![NuGet version](https://img.shields.io/nuget/v/BitzArt.Json.TypedObjects.svg)](https://www.nuget.org/packages/BitzArt.Json.TypedObjects/)
[![NuGet downloads](https://img.shields.io/nuget/dt/BitzArt.Json.TypedObjects.svg)](https://www.nuget.org/packages/BitzArt.Json.TypedObjects/)

## Overview

**BitzArt.Json.TypedObjects** provides utilities, helping to retain objects' original type during serialization and deserialization.

## Installation

Install the following package to your project:

```
dotnet add package BitzArt.Json.TypedObjects
```

## Usage

### TypedObjectJsonConverter

Apply `JsonConverter` attribute to an object property specifying `TypedObjectJsonConverter`:

```csharp
public class YourClass
{
    [JsonConverter(typeof(TypedObjectJsonConverter<object>))]
    public object Value { get; set; }
}
```

This ensures that the `Value` property retains its original type during serialization and deserialization:

```csharp
var instance = new YourClass { Value = 42 };
var serialized = JsonSerializer.Serialize(instance);

var deserialized = JsonSerializer.Deserialize<YourClass>(serialized);
var type = deserialized.Value.GetType(); // System.Int32
```

### ItemConverter

Apply `JsonConverter` attribute to a collection of objects property specifying `ItemConverter` with `TypedObjectJsonConverter` as its generic type:

```csharp
public class YourClass
{
    [JsonConverter(typeof(ItemConverter<TypedObjectJsonConverter<object>>))]
    public IEnumerable<object> Values { get; set; }
}
```

This ensures that each item in the `Values` collection retains its original type information during serialization and deserialization:

```csharp
var instance = new YourClass { Values = [ 42, "Hello"] };
var serialized = JsonSerializer.Serialize(instance);

var deserialized = JsonSerializer.Deserialize<YourClass>(serialized);
var types = deserialized.Values.Select(x => x.GetType()); // System.Int32, System.String
```

## License

[![License](https://img.shields.io/badge/mit-%230072C6?style=for-the-badge)](https://github.com/BitzArt/Miscellaneous/blob/main/LICENSE)
