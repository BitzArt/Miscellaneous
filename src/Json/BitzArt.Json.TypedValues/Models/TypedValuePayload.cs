using System.Text.Json.Serialization;

namespace System.Text.Json;

internal struct TypedValuePayload<T>
{
    private static readonly Dictionary<string, Type> _foundTypes = [];

    public const string TypePropertyName = "type";
    public const string ValuePropertyName = "value";

    [JsonPropertyName(TypePropertyName)]
    [JsonPropertyOrder(0)]
    public string? TypeName { get; private set; }

    [JsonPropertyName(ValuePropertyName)]
    [JsonPropertyOrder(1)]
    public T Value { get; private set; }

    public TypedValuePayload(T value)
        : this(value, value?.GetType().FullName!) { }

    private TypedValuePayload(T value, string? typeName)
    {
        Value = value;
        TypeName = typeName;
    }

    public static TypedValuePayload<T>? Read(JsonElement root, JsonSerializerOptions options)
    {
        if (root.ValueKind == JsonValueKind.Null) return null;

        var actualTypeName = root.GetProperty(TypePropertyName).GetString()!;

        if (string.IsNullOrWhiteSpace(actualTypeName))
        {
            throw new JsonException($"The type name is null or empty.");
        }

        var actualType = Type.GetType(actualTypeName)!;

        if (actualType is null)
        {
            var found = _foundTypes.TryGetValue(actualTypeName, out actualType);
        }

        if (actualType is null)
        {
            actualType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.FullName == actualTypeName);

            if (actualType is not null)
            {
                _foundTypes[actualTypeName] = actualType;
            }
        }

        if (actualType is null)
        {
            throw new JsonException($"The type '{actualTypeName}' could not be found in app domain.");
        }

        if (!actualType.IsAssignableTo(typeof(T)))
        {
            throw new JsonException($"The type '{actualTypeName}' is not assignable to '{typeof(T).FullName}'.");
        }

        var value = (T)root.GetProperty(ValuePropertyName).Deserialize(actualType, options)!;

        return new(value, actualTypeName);
    }
}
