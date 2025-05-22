using System.Text.Json;
using System.Text.Json.Serialization;

namespace BitzArt.Json;

internal class TypePreservingJsonConverter<T> : JsonConverter<T>
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var root = JsonDocument.ParseValue(ref reader).RootElement;
        var payload = TypedValuePayload<T>.Read(root, options);

        if (payload == null)
        {
            return (T?)(object?)null;
        }

        return payload!.Value!.Value;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var payload = new TypedValuePayload<T>(value);
        var converter = (JsonConverter<TypedValuePayload<T>>)options.GetConverter(payload.GetType());
        converter.Write(writer, payload, options);
    }
}
