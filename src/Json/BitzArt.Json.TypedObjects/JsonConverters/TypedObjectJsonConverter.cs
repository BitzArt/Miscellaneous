using System.Text.Json;
using System.Text.Json.Serialization;

namespace BitzArt.Json;

/// <summary>
/// Converts value of type <typeparamref name="T"/> to and from JSON, preserving original type information.
/// </summary>
public class TypedObjectJsonConverter<T> : JsonConverter<T>
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var converter = new TypedValueJsonConverter();
        var typedValue = converter.Read(ref reader, typeToConvert, options);

        if (typedValue is null) return default!;

        return (T)typedValue!.Value!.Value;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
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
