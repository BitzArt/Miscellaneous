using BitzArt.ApiExceptions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MediaMars.Messaging;

/// <summary>
/// Represents a Problem response payload,
/// similar to ProblemDetails (<see href="https://datatracker.ietf.org/doc/html/rfc7807">RFC7807</see>)
/// </summary>
public class Problem
{
    /// <summary>
    /// Problem type.
    /// </summary>
    [JsonPropertyName("type")]
    public string? ErrorType { get; set; }

    /// <summary>
    /// Problem message.
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    /// <summary>
    /// Additional problem details data.
    /// </summary>
    // ToDo: Temporary workaround until https://github.com/dotnet/runtime/issues/60560 is resolved.
    [JsonPropertyName("extensions")]
    public string? ExtensionsJson { get; set; }

    /// <summary>
    /// Additional problem details data.
    /// </summary>
    [JsonIgnore]
    public IDictionary<string, object>? Extensions
        => _extensions ??= ExtensionsJson is not null 
            ? JsonSerializer.Deserialize<IDictionary<string, object>>(ExtensionsJson) 
            : null;

    private IDictionary<string, object>? _extensions;

    /// <summary>
    /// Initializes a new instance of the <see cref="Problem"/> class.
    /// </summary>
    /// <param name="ex"></param>
    public Problem(ApiExceptionBase ex) : this(ex.ErrorType, ex.Message, ex.Payload.Data) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Problem"/> class.
    /// </summary>
    public Problem(string? errorType, string? message, IDictionary<string, object>? data = null) : this()
    {
        ErrorType = errorType;
        Message = message;
        ExtensionsJson = JsonSerializer.Serialize(data);
    }

    /// <summary>
    /// Initializes a new empty instance of the <see cref="Problem"/> class.
    /// This constructor is used in deserialization.
    /// </summary>
    public Problem() { }
}
