using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace BitzArt.TypeInfoResolvers;

internal class MessagingJsonTypeInfoResolver : DefaultJsonTypeInfoResolver
{
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

        IgnoreMarkedProperties(jsonTypeInfo);

        return jsonTypeInfo;
    }

    /// <summary>
    /// Marks properties with the [MessageIgnore] attribute as ignored.
    /// </summary>
    /// <param name="jsonTypeInfo"></param>
    private static void IgnoreMarkedProperties(JsonTypeInfo jsonTypeInfo)
    {
        var attributeType = typeof(MessageIgnoreAttribute);

        var propertiesToIgnore = jsonTypeInfo.Properties.Where(x =>
                x.AttributeProvider!.GetCustomAttributes(attributeType, true).Length != 0)
            .ToList();

        propertiesToIgnore.ForEach(x => x.ShouldSerialize = (_, _) => false);
    }
}