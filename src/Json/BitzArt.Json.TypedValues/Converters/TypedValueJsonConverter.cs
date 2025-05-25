using System.Text.Json.Serialization;

namespace System.Text.Json;

/// <summary>
/// Preserves type information when serializing values,
/// allowing for correct deserialization back to their original types.
/// </summary>
public class TypedValueJsonConverter : JsonConverterFactory
{
    /// <inheritdoc/>
    public override bool CanConvert(Type typeToConvert) => true;

    /// <inheritdoc/>
    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = GetConverterType(typeToConvert);
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }

    private static Type GetConverterType(Type typeToConvert)
    {
        if (typeToConvert == typeof(TypedValue))
        {
            return typeof(TypedValueJsonConverter<object>);
        }

        if (typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(TypedValue<>))
        {
            var valueType = typeToConvert.GenericTypeArguments[0];
            var converterType = typeof(TypedValueJsonConverter<>).MakeGenericType(valueType);
            return converterType;
        }

        return typeof(TypePreservingJsonConverter<>).MakeGenericType(typeToConvert);
    }
}
