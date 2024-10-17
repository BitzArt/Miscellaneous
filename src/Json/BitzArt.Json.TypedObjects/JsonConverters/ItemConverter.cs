// Source of the code: https://stackoverflow.com/a/67082032

using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BitzArt.Json;

/// <summary>
/// Converts a collection to and from JSON using a specified <typeparamref name="TItemConverter"/>.
/// </summary>
/// <typeparam name="TItemConverter"></typeparam>
public class ItemConverter<TItemConverter> : JsonConverterFactory
    where TItemConverter : JsonConverter, new()
{
    readonly TItemConverter itemConverter = new();

    /// <summary>
    /// <inheritdoc/>/>
    /// </summary>
    public override bool CanConvert(Type typeToConvert) => GetItemType(typeToConvert).ItemType is var itemType && itemType != null && itemConverter.CanConvert(itemType);

    /// <summary>
    /// <inheritdoc/>/>
    /// </summary>
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var (itemType, isArray, isSet) = GetItemType(typeToConvert);

        if (itemType == null)
            throw new NotImplementedException();

        if (isArray)
            return (JsonConverter)Activator.CreateInstance(typeof(ArrayItemConverterDecorator<>).MakeGenericType(typeof(TItemConverter), itemType), [options, itemConverter])!;

        if (!typeToConvert.IsAbstract && !typeToConvert.IsInterface && typeToConvert.GetConstructor(Type.EmptyTypes) != null)
            return (JsonConverter)Activator.CreateInstance(typeof(ConcreteCollectionItemConverterDecorator<,,>).MakeGenericType(typeof(TItemConverter), typeToConvert, typeToConvert, itemType), [options, itemConverter])!;

        if (isSet)
        {
            var setType = typeof(HashSet<>).MakeGenericType(itemType);
            if (typeToConvert.IsAssignableFrom(setType))
                return (JsonConverter)Activator.CreateInstance(typeof(ConcreteCollectionItemConverterDecorator<,,>).MakeGenericType(typeof(TItemConverter), setType, typeToConvert, itemType), [options, itemConverter])!;
        }
        else
        {
            var listType = typeof(List<>).MakeGenericType(itemType);
            if (typeToConvert.IsAssignableFrom(listType))
                return (JsonConverter)Activator.CreateInstance(typeof(ConcreteCollectionItemConverterDecorator<,,>).MakeGenericType(typeof(TItemConverter), listType, typeToConvert, itemType), [options, itemConverter])!;
        }

        // OK it's not an array and we can't find a parameterless constructor for the type. We can serialize, but not deserialize.
        return (JsonConverter)Activator.CreateInstance(typeof(EnumerableItemConverterDecorator<,>).MakeGenericType(typeof(TItemConverter), typeToConvert, itemType), [options, itemConverter])!;
    }

    private static (Type? ItemType, bool IsArray, bool isSet) GetItemType(Type type)
    {
        // Quick reject for performance
        // Dictionary is not implemented. 
        if (type.IsPrimitive || type == typeof(string) || typeof(IDictionary).IsAssignableFrom(type))
            return (null, false, false);

        if (type.IsArray)
            return type.GetArrayRank() == 1 ? (type.GetElementType(), true, false) : (null, false, false);

        Type? itemType = null;
        bool isSet = false;

        foreach (var iType in type.GetInterfacesAndSelf())
        {
            if (iType.IsGenericType)
            {
                var genType = iType.GetGenericTypeDefinition();
                if (genType == typeof(ISet<>))
                {
                    isSet = true;
                }
                else if (genType == typeof(IEnumerable<>))
                {
                    var thisItemType = iType.GetGenericArguments()[0];

                    if (itemType != null && itemType != thisItemType)
                        return (null, false, false); // Type implements multiple enumerable types simultaneously. 

                    itemType = thisItemType;
                }
                else if (genType == typeof(IDictionary<,>))
                {
                    return (null, false, false);
                }
            }
        }

        return (itemType, false, isSet);
    }

    private abstract class CollectionItemConverterDecoratorBase<TEnumerable, TItem> : JsonConverter<TEnumerable>
        where TEnumerable : IEnumerable<TItem>
    {
        readonly JsonConverter<TItem> innerConverter;

        public CollectionItemConverterDecoratorBase(JsonSerializerOptions options, TItemConverter converter)
        {
            // Clone the incoming options and insert the item converter at the beginning of the clone.
            // Then if converter is actually a JsonConverterFactory (e.g. JsonStringEnumConverter) then the correct JsonConverter<T> will be manufactured or fetched.
            var modifiedOptions = new JsonSerializerOptions(options);
            modifiedOptions.Converters.Insert(0, converter);
            innerConverter = (JsonConverter<TItem>)modifiedOptions.GetConverter(typeof(TItem));
        }

        protected TCollection BaseRead<TCollection>(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            where TCollection : ICollection<TItem>, new()
        {
            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException(); // Unexpected token type. JsonTokenType.Null is handled by the framework, unless we set HandleNull => true (which we didn't).

            var list = new TCollection();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                    break;
                var item = innerConverter.Read(ref reader, typeof(TItem), options);

                // TODO: optionally add checks to make sure innerConverter correctly advanced the reader to the end of the current token.
                list.Add(item!);
            }

            return list;
        }

        public sealed override void Write(Utf8JsonWriter writer, TEnumerable value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            foreach (var item in value)
                innerConverter.Write(writer, item, options);

            writer.WriteEndArray();
        }
    }

    private sealed class ArrayItemConverterDecorator<TItem>(JsonSerializerOptions options, TItemConverter converter)
        : CollectionItemConverterDecoratorBase<TItem[], TItem>(options, converter)
    {
        public override TItem[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => BaseRead<List<TItem>>(ref reader, typeToConvert, options).ToArray();
    }

    private sealed class ConcreteCollectionItemConverterDecorator<TCollection, TEnumerable, TItem>(JsonSerializerOptions options, TItemConverter converter)
        : CollectionItemConverterDecoratorBase<TEnumerable, TItem>(options, converter)
        where TCollection : ICollection<TItem>, TEnumerable, new()
        where TEnumerable : IEnumerable<TItem>
    {
        public override TEnumerable Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => BaseRead<TCollection>(ref reader, typeToConvert, options);
    }

    private sealed class EnumerableItemConverterDecorator<TEnumerable, TItem>(JsonSerializerOptions options, TItemConverter converter)
        : CollectionItemConverterDecoratorBase<TEnumerable, TItem>(options, converter) where TEnumerable : IEnumerable<TItem>
    {
        public override TEnumerable Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => throw new NotImplementedException(string.Format("Deserialization is not implemented for type {0}", typeof(TEnumerable)));
    }
}

/// <summary>
/// Extension methods for <see cref="Type"/>.
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// Retrieves all interfaces implemented by the specified type.
    /// </summary>
    public static IEnumerable<Type> GetInterfacesAndSelf(this Type type) =>
        (type ?? throw new ArgumentNullException(nameof(type))).IsInterface
            ? new[] { type }.Concat(type.GetInterfaces())
            : type.GetInterfaces();
}
