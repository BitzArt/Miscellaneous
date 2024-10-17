using System.Text.Json;
using System.Text.Json.Serialization;

namespace BitzArt.Json;

public class TypedObjectJsonConverter<T> : JsonConverter<T>
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var converter = new TypedValueJsonConverter();
        var typedValue = converter.Read(ref reader, typeToConvert, options);

        if (typedValue is null) return default!;

        return (T)typedValue!.Value!.Value;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        var typedJsonValue = new TypedObjectJsonValue(value!);
        var converter = new TypedValueJsonConverter();
        converter.Write(writer, typedJsonValue, options);
    }
}
