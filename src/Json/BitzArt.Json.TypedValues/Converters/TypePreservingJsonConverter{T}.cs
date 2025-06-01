using System.Text.Json.Serialization;

namespace System.Text.Json;

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
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        var payloadType = typeof(TypedValuePayload<>).MakeGenericType(value.GetType());
        var payload = Activator.CreateInstance(payloadType, value)!;
        var converter = options.GetConverter(payloadType);
        var write = converter.GetType().GetMethod("Write", [typeof(Utf8JsonWriter), payloadType, typeof(JsonSerializerOptions)])!;
        write.Invoke(converter, [writer, payload, options]);
    }
}
