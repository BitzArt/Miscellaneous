using System.Text.Json;
using System.Text.Json.Serialization;

namespace BitzArt.Json.TypedObjects.Tests;

public class TypedValueJsonConverterTests
{
    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(int.MinValue)]
    [InlineData(int.MaxValue)]
    [InlineData(long.MinValue)]
    [InlineData(long.MaxValue)]
    [InlineData(short.MinValue)]
    [InlineData(short.MaxValue)]
    [InlineData(byte.MinValue)]
    [InlineData(byte.MaxValue)]
    [InlineData(' ')]
    [InlineData('a')]
    [InlineData('A')]
    [InlineData('0')]
    [InlineData('9')]
    [InlineData(0.0)]
    [InlineData(1.0)]
    [InlineData(double.MinValue)]
    [InlineData(double.MaxValue)]
    [InlineData(0.0f)]
    [InlineData(1.0f)]
    [InlineData(float.MinValue)]
    [InlineData(float.MaxValue)]
    [InlineData("a")]
    [InlineData("A")]
    [InlineData("0")]
    [InlineData("9")]
    [InlineData(" ")]
    [InlineData("aA0 9")]
    [InlineData("aA0 9!")]
    [InlineData("aA0 9!@#$%^&*()_+")]
    [InlineData("aA0 9!@#$%^&*()_+[]{}|;':\",./<>?")]
    public void Serialization_TypedValue_ShouldRetainType(object? value)
    {
        // Arrange
        var typed = TypedValue.From(value);

        // Act
        var serialized = JsonSerializer.Serialize(typed);
        var deserialized = JsonSerializer.Deserialize<TypedValue>(serialized);

        // Assert
        var result = deserialized?.Value;

        Assert.Equal(value?.GetType(), result?.GetType());
        Assert.Equal(value, result);
    }

    [Fact]
    public void Serialization_TypedValueOnBaseClass_ShouldRetainType()
    {
        // Arrange
        var apple = new Apple("red");
        var fruit = (Fruit)apple;
        var typed = TypedValue.From(fruit);

        // Act
        var serialized = JsonSerializer.Serialize(typed);
        var deserialized = JsonSerializer.Deserialize<TypedValue<Fruit>>(serialized);

        // Assert
        Assert.Equal(apple.GetType(), deserialized?.Value?.GetType());
        Assert.Equal(apple, deserialized?.Value);
    }

    [Fact]
    public void Serialization_TypedValueCollection_ShouldRetainTypes()
    {
        // Arrange
        var fruits = new List<TypedValue<Fruit>> { new Apple("red"), new Banana() };

        // Act
        var serialized = JsonSerializer.Serialize(fruits);
        var deserialized = JsonSerializer.Deserialize<List<TypedValue<Fruit>>>(serialized);

        // Assert
        Assert.Equal(fruits.Count, deserialized?.Count);
        
        for (int i = 0; i < fruits.Count; i++)
        {
            var original = fruits[i];
            var result = deserialized![i];

            Assert.Equal(original.Value.GetType(), result.Value?.GetType());
            Assert.Equal(original.Value, result.Value);
        }
    }

    [Fact]
    public void Serialization_MarkedClass_ShouldRetainType()
    {
        // Arrange
        var text = "Some text";
        var markedClass = new MarkedClassChild(text);
        var castDown = (object)markedClass;

        // Act
        var serialized = JsonSerializer.Serialize(castDown);
        var deserialized = JsonSerializer.Deserialize<MarkedClassBase>(serialized);

        // Assert
        Assert.Equal(markedClass.GetType(), deserialized?.GetType());
        Assert.Equal(markedClass, deserialized);
    }

    [JsonConverter(typeof(TypedValueJsonConverter))]
    private record MarkedClassBase;

    private record MarkedClassChild(string Text) : MarkedClassBase;
}
