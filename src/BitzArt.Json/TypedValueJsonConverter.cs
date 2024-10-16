using System.Text.Json;
using System.Text.Json.Serialization;

namespace BitzArt.Json;

internal class TypedValueJsonConverter : JsonConverter<TypedObjectJsonValue?>
{
    public override TypedObjectJsonValue? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonDocument = JsonDocument.ParseValue(ref reader);
        var rootElement = jsonDocument.RootElement;

        if (rootElement.ValueKind == JsonValueKind.Null)
            return null;

        var typeName = rootElement.GetProperty("type").GetString()!;
        var type = Type.GetType(typeName)!;

        var value = rootElement.GetProperty("value").Deserialize(type, options)!;

        return new TypedObjectJsonValue(typeName, value);
    }

    public override void Write(Utf8JsonWriter writer, TypedObjectJsonValue? value, JsonSerializerOptions options)
    {
        var converter = (JsonConverter<TypedObjectJsonValue?>)options.GetConverter(typeof(TypedObjectJsonValue?));
        converter.Write(writer, value, options);
    }
}
