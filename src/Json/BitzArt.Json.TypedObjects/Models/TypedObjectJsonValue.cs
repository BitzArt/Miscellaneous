using System.Text.Json.Serialization;

namespace BitzArt.Json;

internal struct TypedObjectJsonValue
{
    [JsonPropertyName("value")]
    public object Value { get; private set; }

    [JsonPropertyName("type")]
    public string TypeName { get; private set; }

    public TypedObjectJsonValue(object value)
    {
        Value = value;
        TypeName = value.GetType().FullName!;
    }

    public TypedObjectJsonValue(string typeName, object value)
    {
        Value = value;
        TypeName = typeName;
    }
}
