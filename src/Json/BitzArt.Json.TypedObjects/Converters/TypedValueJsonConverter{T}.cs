using System.Text.Json;
using System.Text.Json.Serialization;

namespace BitzArt.Json;

internal class TypedValueJsonConverter<T> : JsonConverter<TypedValue<T>>
{
    public override TypedValue<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var root = JsonDocument.ParseValue(ref reader).RootElement;
        var payload = TypedValuePayload<T>.Read(root, options);

        if (payload == null)
        {
            return null;
        }

        return TypedValue<T>.FromPayload(payload!.Value);
    }

    public override void Write(Utf8JsonWriter writer, TypedValue<T> value, JsonSerializerOptions options)
    {
        if (value.Value is null)
        {
            writer.WriteNullValue();
            return;
        }

        var payload = value.ToPayload();
        var converter = (JsonConverter<TypedValuePayload<T>>)options.GetConverter(payload.GetType());
        converter.Write(writer, payload, options);
    }
}
