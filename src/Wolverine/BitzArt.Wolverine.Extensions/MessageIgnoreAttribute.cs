namespace BitzArt;

/// <summary>
/// Prevents a property from being serialized into a message or deserialized from a message.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class MessageIgnoreAttribute : Attribute
{
}