using System.Text.Json;
using System.Text.Json.Serialization;

namespace BitzArt.Json.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        // Arrange
        var testClass = new TestClass1([1, 2L, 3.0f, 4.0, 5.0m, "6", Guid.NewGuid(), true, false]);
        var types = testClass.Values.Select(x => x.GetType()).ToList();

        var serialized = JsonSerializer.Serialize(testClass);

        // Act
        var deserialized = JsonSerializer.Deserialize<TestClass1>(serialized)!;
        var deserializedTypes = deserialized.Values.Select(x => x.GetType()).ToList();

        // Assert
        Assert.Equal(testClass.Values, deserialized.Values);
        Assert.Equal(types, deserializedTypes);
    }

    private class TestClass1
    {
        [JsonConverter(typeof(ItemConverter<TypedObjectJsonConverter<object>>))]
        public IEnumerable<object> Values { get; set; }

        public TestClass1(IEnumerable<object> values)
        {
            Values = values;
        }

        public TestClass1()
        {
        }
    }

    [Fact]
    public void Test2()
    {
        // Arrange
        var testClass = new TestClass2([1, 2L, 3.0f, 4.0, 5.0m, "6", Guid.NewGuid(), true, false, null]);
        var types = testClass.Values.Select(x => x?.GetType()).ToList();

        var serialized = JsonSerializer.Serialize(testClass);

        // Act
        var deserialized = JsonSerializer.Deserialize<TestClass2>(serialized)!;
        var deserializedTypes = deserialized.Values.Select(x => x?.GetType()).ToList();

        // Assert
        Assert.Equal(testClass.Values, deserialized.Values);
        Assert.Equal(types, deserializedTypes);
    }

    private class TestClass2
    {
        [JsonConverter(typeof(ItemConverter<TypedObjectJsonConverter<object?>>))]
        public IEnumerable<object?> Values { get; set; }

        public TestClass2(IEnumerable<object?> values)
        {
            Values = values;
        }

        public TestClass2()
        {
        }
    }
}
